using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;
using System.IO;
using System.Management;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;


namespace PrometheusActivator.Utilities
{
    public static class PowershellHandler
    {
        public static string RunCommand(string cmd)
        {
            using PowerShell ps = PowerShell.Create();
            ps.AddScript(cmd);
            try
            {
                var results = ps.Invoke();
                if (ps.HadErrors)
                {
                    IEnumerable<string> errors = results.Select(o => o.ToString());
                    return $"Error executing command: {cmd}{Environment.NewLine}{string.Join(Environment.NewLine, errors)}";
                }

                return string.Join(Environment.NewLine, results.Select(o => o.ToString()));
            }
            catch (Exception ex)
            {
                return $"Exception occurred: {ex.Message}";
            }
            finally
            {
                ps.Dispose();
            }
        }

        public static async Task<string> RunCommandAsync(string cmd)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                using Runspace runspace = RunspaceFactory.CreateRunspace();
                runspace.Open();
                using PowerShell ps = PowerShell.Create();
                ps.Runspace = runspace;
                ps.AddScript(cmd);
                try
                {
                    var results = ps.Invoke();
                    if (ps.HadErrors)
                    {
                        IEnumerable<string> errors = results.Select(o => o.ToString());
                        return $"Error executing command: {cmd}{Environment.NewLine}{string.Join(Environment.NewLine, errors)}";
                    }

                    return string.Join(Environment.NewLine, results.Select(o => o.ToString()));
                }
                catch (Exception ex)
                {
                    return $"Exception occurred: {ex.Message}";
                }
                finally
                {
                    runspace.Close();
                }
            });
        }
    }

    public static class RegistryHandler
    {
        public static void SafeDeleteSubKeyTree(RegistryKey? key, string subKey)
        {
            if (key != null && key.GetSubKeyNames().Contains(subKey))
                key.DeleteSubKeyTree(subKey);
        }

        public static void SafeDeleteValue(RegistryKey? key, string valueName)
        {
            if (key != null && key.GetValue(valueName) != null)
                key.DeleteValue(valueName);
        }
    }


    public static class InternetConnection
    {
        private static readonly string[] ReliableHosts =
        {
            "https://www.google.com",
            "https://www.microsoft.com",
            "https://www.cloudflare.com"
        };

        public static async Task<bool> IsInternetAvailable()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return false;

            try
            {
                using HttpClient client = new();
                client.Timeout = TimeSpan.FromSeconds(5);
                client.DefaultRequestHeaders.ConnectionClose = true;

                foreach (string host in ReliableHosts)
                {
                    try
                    {
                        using HttpResponseMessage response = await client.GetAsync(host);
                        if (response.IsSuccessStatusCode)
                            return true;
                    }
                    catch
                    {
                        continue;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }

    public static class WMI_Handler
    {
        public static string GetProperty(string query, string property)
        {
            foreach (ManagementObject obj in new ManagementObjectSearcher(query).Get().Cast<ManagementObject>())
            {
                if (obj[property] is string prop && !string.IsNullOrEmpty(prop))
                {
                    return prop;
                }
            }
            return string.Empty;
        }

        // Yeh, a guffy solution from my first college days with python
        public static string InvokeMethod(string query, string method, string returnMsg)
        {
            foreach (ManagementObject instance in new ManagementObjectSearcher(query).Get().Cast<ManagementObject>())
            {
                using ManagementBaseObject outParams = instance.InvokeMethod(method, null, null);
                int returnValue = Convert.ToInt32(outParams["ReturnValue"]);
                return returnValue == 0 ? returnMsg : $"Error: {returnValue}";
            }

            return string.Empty;
        }

        public static string InvokeMethod_Args(string query, string method, string argument, string parameter, string returnMsg)
        {
            foreach (ManagementObject service in new ManagementObjectSearcher(query).Get().Cast<ManagementObject>())
            {
                using ManagementBaseObject inParams = service.GetMethodParameters(method);
                inParams[argument] = parameter;

                using ManagementBaseObject outParams = service.InvokeMethod(method, inParams, null);
                int returnValue = Convert.ToInt32(outParams["ReturnValue"]);
                return returnValue == 0 ? returnMsg : $"Error: {returnValue}";
            }

            return string.Empty;
        }
    }

    public class RenewTask
    {
        public string script;
        public string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PrometheusActivator");
        public string scriptFilePath;
        public const string taskFolderName = "\\PrometheusActivator";
        public TaskService ts = new();
        public TaskFolder tf;

        public RenewTask(string scriptName)
        {
            script = scriptName;
            scriptFilePath = Path.Combine(appDataPath, $"{script}.ps1");
        }

        private void SaveScript()
        {
            Directory.CreateDirectory(appDataPath);

            if (!File.Exists(scriptFilePath))
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string resourceName = $"OpenFuryKMS.Scripts.{script}.ps1";
                if (assembly.GetManifestResourceNames().Contains(resourceName))
                {
                    using Stream? stream = assembly.GetManifestResourceStream(resourceName);
                    using FileStream fileStream = new(scriptFilePath, FileMode.Create, FileAccess.Write);
                    stream?.CopyTo(fileStream);
                }
            }
        }

        public bool IsTaskScheduled()
        {
            try
            {
                GetTaskFolder();
                var task = tf?.GetTasks().FirstOrDefault(t => t.Name == script);
                return task != null;
            }
            catch
            {
                return false;
            }
        }

        public string CreateScheduledTask()
        {
            SaveScript();
            try
            {
                GetTaskFolder();
                tf ??= ts.RootFolder.CreateFolder(taskFolderName);

                if (tf.GetTasks().FirstOrDefault(t => t.Name == script) == null)
                {
                    TaskDefinition taskDefinition = ts.NewTask();
                    taskDefinition.RegistrationInfo.Description = $"Executes the script {script}.ps1 every 181 days.";
                    taskDefinition.Triggers.Add(new DailyTrigger { DaysInterval = 181 });
                    taskDefinition.Actions.Add(new ExecAction("powershell.exe", $"-ExecutionPolicy Bypass -File \"{scriptFilePath}\"", null));
                    tf.RegisterTaskDefinition(script, taskDefinition);
                }
                return "Task created successfully.";
            }
            catch (Exception ex)
            {
                return $"Error creating the task: {ex.Message}";
            }
        }


        public void DeleteTask()
        {
            if (File.Exists(scriptFilePath))
            {
                File.Delete(scriptFilePath);
            }

            GetTaskFolder();
            if (tf != null)
            {
                Microsoft.Win32.TaskScheduler.Task task = tf.GetTasks().FirstOrDefault(t => t.Name == script);

                if (task != null)
                {
                    tf.DeleteTask(script);
                }
            }
        }

        public void CleanAll()
        {
            DeleteTask();

            if (Directory.GetFiles(appDataPath) == null)
            {
                Directory.Delete(appDataPath);
            }

            GetTaskFolder();
            tf?.DeleteFolder(taskFolderName);
        }

        private void GetTaskFolder() => tf = ts.GetFolder(taskFolderName);

        public void RecreateTask()
        {
            DeleteTask();
            CreateScheduledTask();
        }
    }
}

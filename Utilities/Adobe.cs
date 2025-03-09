using NetFwTypeLib;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;


namespace PrometheusActivator.Utilities
{
    public class AdobeProduct : INotifyPropertyChanged
    {
        private bool _isFirewallBlocked;
        private bool _isUpdating;


        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsFirewallBlocked
        {
            get => _isFirewallBlocked;
            set
            {
                if (_isFirewallBlocked != value && !_isUpdating)
                {
                    _isUpdating = true;
                    UpdateFirewallStateAsync(value).ConfigureAwait(false);
                }
            }
        }

        private async Task UpdateFirewallStateAsync(bool value)
        {
            try
            {
                bool success;
                if (value)
                    success = await AdobeHandler.AddFirewallBlockRule(this);
                else
                    success = await AdobeHandler.RemoveFirewallRule(Name);

                if (success)
                {
                    _isFirewallBlocked = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFirewallBlocked)));
                }
            }
            finally
            {
                _isUpdating = false;
            }
        }

        public string Name { get; set; }
        public string Version { get; set; }
        public string ExecutablePath { get; set; }
        public BitmapImage Icon { get; set; }
    }

    public static class AdobeHandler
    {
        public static List<AdobeProduct> Products = new() { };
        private static readonly string[] AdobePaths =
        {
            Environment.ExpandEnvironmentVariables(@"%PROGRAMFILES%\Adobe"),
            Environment.ExpandEnvironmentVariables(@"%PROGRAMFILES(x86)%\Adobe")
        };

        private static readonly HashSet<string> ExecutableNames = new()
        {
            "Photoshop.exe",
            "Illustrator.exe",
            "AfterFX.exe",
            "Premiere.exe",
            "InDesign.exe"
        };

        private static IEnumerable<string> FindExecutables()
        {
            foreach (var path in AdobePaths)
            {
                if (!Directory.Exists(path)) continue;
                var folders = Directory.EnumerateDirectories(path);
                foreach (var folder in folders)
                {
                    var executables = Directory.EnumerateFiles(folder, "*.exe", SearchOption.AllDirectories)
                                               .Where(file => ExecutableNames.Contains(Path.GetFileName(file)));
                    foreach (var executable in executables)
                    {
                        yield return executable;
                    }
                }
            }
        }

        public static async Task LoadProducts()
        {
            Products.Clear();
            var executables = FindExecutables();
            foreach (var executable in executables)
            {
                FileVersionInfo info = FileVersionInfo.GetVersionInfo(executable);
                string productName = info.ProductName ?? Path.GetFileNameWithoutExtension(executable);

                AdobeProduct product = new()
                {
                    Name = productName,
                    Version = info.ProductVersion ?? "1.0.0",
                    ExecutablePath = executable,
                    IsFirewallBlocked = await FirewallRuleExists(productName),
                    Icon = await Task.Run(() => GetLargeIconAsImageSource(executable))
                };
                Products.Add(product);
            }
        }


        public static async Task<bool> FirewallRuleExists(string ruleName)
        {
            try
            {
                return await Task.Run(() =>
                {
                    INetFwPolicy2 fwPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                    bool hasInbound = false;
                    bool hasOutbound = false;

                    foreach (INetFwRule rule in fwPolicy.Rules)
                    {
                        if (rule.Name == ruleName)
                        {
                            if (rule.Direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN)
                            {
                                hasInbound = true;
                            }
                            else if (rule.Direction == NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT)
                            {
                                hasOutbound = true;
                            }

                            if (hasInbound && hasOutbound) break;
                        }
                    }


                    return hasInbound && hasOutbound;
                });
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> AddFirewallBlockRule(AdobeProduct product)
        {
            try
            {
                return await Task.Run(async () =>
                {
                    if (await FirewallRuleExists(product.Name))
                    {
                        await RemoveFirewallRule(product.Name);
                    }

                    INetFwPolicy2 fwPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                    Type ruleType = Type.GetTypeFromProgID("HNetCfg.FwRule");

                    var tasks = new[]
                    {
                        Task.Run(() => CreateInboundRule(product, ruleType)),
                        Task.Run(() => CreateOutboundRule(product, ruleType))
                    };

                    var rules = await Task.WhenAll(tasks);

                    foreach (var rule in rules)
                    {
                        fwPolicy.Rules.Add(rule);
                    }

                    product.IsFirewallBlocked = true;
                    return true;
                });
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> RemoveFirewallRule(string ruleName)
        {
            try
            {
                return await Task.Run(() =>
                {
                    INetFwPolicy2 fwPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

                    foreach (string name in fwPolicy.Rules.Cast<INetFwRule>().Where(rule => rule.Name.Contains(ruleName))
                                                    .Select(rule => rule.Name).ToList())
                    {
                        fwPolicy.Rules.Remove(name);
                    }

                    var product = Products.FirstOrDefault(p => p.Name == ruleName);
                    if (product != null)
                    {
                        product.IsFirewallBlocked = false;
                    }

                    return true;
                });
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static INetFwRule CreateInboundRule(AdobeProduct product, Type ruleType)
        {
            INetFwRule rule = (INetFwRule)Activator.CreateInstance(ruleType);
            rule.Name = $"{product.Name}";
            rule.Description = $"Blocks inbound network access for {product.Name}, allowing you to use the software for free at the cost of losing online functionalities.";
            rule.ApplicationName = product.ExecutablePath;
            rule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
            rule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
            rule.Enabled = true;
            rule.Profiles = (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL;
            return rule;
        }

        private static INetFwRule CreateOutboundRule(AdobeProduct product, Type ruleType)
        {
            INetFwRule rule = (INetFwRule)Activator.CreateInstance(ruleType);
            rule.Name = $"{product.Name}";
            rule.Description = $"Blocks outbound network access for {product.Name}, allowing you to use the software for free at the cost of losing online functionalities.";
            rule.ApplicationName = product.ExecutablePath;
            rule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
            rule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
            rule.Enabled = true;
            rule.Profiles = (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL;
            return rule;
        }


        private static BitmapImage GetLargeIconAsImageSource(string executablePath)
        {
            try
            {
                using var icon = System.Drawing.Icon.ExtractAssociatedIcon(executablePath);
                if (icon == null)
                    return null;

                using var bitmap = icon.ToBitmap();
                using var memoryStream = new MemoryStream();
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
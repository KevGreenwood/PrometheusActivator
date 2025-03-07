using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;
using NetFwTypeLib;


namespace PrometheusActivator.Utilities
{
    public class AdobeProduct : INotifyPropertyChanged
    {
        private bool _isFirewallBlocked;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsFirewallBlocked
        {
            get => _isFirewallBlocked;
            set
            {
                if (_isFirewallBlocked != value)
                {
                    _isFirewallBlocked = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFirewallBlocked)));

                    if (value)
                        AdobeHandler.AddFirewallBlockRule(this);
                    else
                        AdobeHandler.RemoveFirewallRule(Name);
                }
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
            @"C:\Program Files\Adobe",
            @"C:\Program Files (x86)\Adobe"
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
                var productName = info.ProductName ?? Path.GetFileNameWithoutExtension(executable);

                var product = new AdobeProduct
                {
                    Name = productName,
                    Version = info.ProductVersion ?? "Unknown",
                    ExecutablePath = executable,
                    IsFirewallBlocked = await CheckFirewallRuleAsync(productName),
                    Icon = await Task.Run(() => GetLargeIconAsImageSource(executable))
                };
                Products.Add(product);
            }
        }

        public static async Task<bool> CheckFirewallRuleAsync(string ruleName)
        {
            return await Task.Run(() => FirewallRuleExists(ruleName));
        }

        public static bool FirewallRuleExists(string ruleName)
        {
            try
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
                    }
                }

                return hasInbound && hasOutbound;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al verificar reglas de firewall: {ex.Message}");
                return false;
            }
        }


        public static bool AddFirewallBlockRule(AdobeProduct product)
        {
            try
            {
                if (FirewallRuleExists(product.Name))
                {
                    RemoveFirewallRule(product.Name);
                }

                INetFwPolicy2 fwPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

                Type ruleType = Type.GetTypeFromProgID("HNetCfg.FwRule");

                INetFwRule inboundRule = (INetFwRule)Activator.CreateInstance(ruleType);
                inboundRule.Name = $"{product.Name}";
                inboundRule.Description = $"Blocks inbound network access for {product.Name}, allowing you to use the software for free at the cost of losing online functionalities.";
                inboundRule.ApplicationName = product.ExecutablePath;
                inboundRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                inboundRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN;
                inboundRule.Enabled = true;
                inboundRule.Profiles = (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL;

                INetFwRule outboundRule = (INetFwRule)Activator.CreateInstance(ruleType);
                outboundRule.Name = $"{product.Name}";
                outboundRule.Description = $"Blocks outbound network access for {product.Name}, allowing you to use the software for free at the cost of losing online functionalities.";
                outboundRule.ApplicationName = product.ExecutablePath;
                outboundRule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                outboundRule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
                outboundRule.Enabled = true;
                outboundRule.Profiles = (int)NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_ALL;

                fwPolicy.Rules.Add(inboundRule);
                fwPolicy.Rules.Add(outboundRule);

                product.IsFirewallBlocked = true;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al agregar reglas de firewall: {ex.Message}");
                return false;
            }
        }


        public static bool RemoveFirewallRule(string ruleName)
        {
            try
            {
                INetFwPolicy2 fwPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

                List<string> rulesToRemove = new();

                foreach (INetFwRule rule in fwPolicy.Rules)
                {
                    if (rule.Name.Contains(ruleName))
                    {
                        rulesToRemove.Add(rule.Name);
                    }
                }

                foreach (string name in rulesToRemove)
                {
                    fwPolicy.Rules.Remove(name);
                }

                var product = Products.FirstOrDefault(p => p.Name == ruleName);
                if (product != null)
                {
                    product.IsFirewallBlocked = false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al eliminar regla de firewall: {ex.Message}");
                return false;
            }
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
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al cargar el icono: {ex.Message}");
                return null;
            }
        }
    }
}
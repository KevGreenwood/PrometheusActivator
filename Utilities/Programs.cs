using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;


namespace PrometheusActivator.Utilities
{
    public class Product
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string ExecutablePath { get; set; }
        public bool IsFirewallBlocked { get; set; }
        public BitmapImage Icon { get; set; }
    }

    public static class ProductHandler
    {
        public static ObservableCollection<Product> Products { get; } = new();
        private static readonly string[] CommonPaths =
        {
            Environment.ExpandEnvironmentVariables(@"%PROGRAMFILES%\Adobe"),
            Environment.ExpandEnvironmentVariables(@"%PROGRAMFILES(x86)%\Adobe")
        };

        private static HashSet<string> ExecutableNames = new()
        {
            "Photoshop.exe",
            "Illustrator.exe",
            "AfterFX.exe",
            "Premiere.exe",
            "InDesign.exe"
        };

        private static IEnumerable<string> FindExecutables()
        {
            foreach (var path in CommonPaths)
            {
                if (!Directory.Exists(path)) continue;
                IEnumerable<string> folders = Directory.EnumerateDirectories(path);
                foreach (var folder in folders)
                {
                    IEnumerable<string> executables = Directory.EnumerateFiles(folder, "*.exe", SearchOption.AllDirectories)
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
            IEnumerable<string> executables = FindExecutables();
            foreach (string executable in executables)
            {
                FileVersionInfo info = FileVersionInfo.GetVersionInfo(executable);
                string productName = info.ProductName ?? Path.GetFileNameWithoutExtension(executable);

                Product product = new()
                {
                    Name = productName,
                    Version = info.ProductVersion ?? "1.0.0",
                    ExecutablePath = executable,
                    IsFirewallBlocked = FirewallManager.FirewallRuleExists(),
                    Icon = await Task.Run(() => GetIcon(executable))
                };
                Products.Add(product);
            }
        }

        public static BitmapImage GetIcon(string executablePath)
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
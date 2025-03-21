using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;

namespace PrometheusActivator.Utilities
{
    public static class WindowsHandler
    {
        private static RegistryKey WindowsRK = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false);
        private static string ProductName = WindowsRK.GetValue("ProductName").ToString();
        private static string EditionID = WindowsRK.GetValue("EditionID").ToString();

        public static int Build => Convert.ToInt32(WindowsRK.GetValue("CurrentBuildNumber").ToString());
        public static string Platform => Environment.Is64BitOperatingSystem ? "64 bits" : "32 bits";
        private static string DisplayVersion { get; set; }
        private static string UBR { get; set; }
        public static string Version { get; set; }
        public static string GetMinimalInfo { get; set; }
        public static string GetAllInfo { get; private set; }

        public static Uri Logo { get; set; }
        public static BitmapImage LogoPNG { get; private set; }

        public static async Task InitializeAsync()
        {
            switch (ProductName)
            {
                case string p when p.Contains("Windows 7"):
                    UBR = WindowsRK.GetValue("CSDVersion").ToString();
                    Logo = new Uri("pack://application:,,,/Assets/SVG/Windows/7.svg");
                    Version = $"{UBR} ({Build})";
                    GetMinimalInfo = $"{ProductName} {Platform}";

                    break;

                case string p when p.Contains("Windows 8"):
                    Logo = new Uri("pack://application:,,,/Assets/SVG/Windows/10.svg");
                    break;

                case string p when p.Contains("Windows 10"):
                    DisplayVersion = WindowsRK.GetValue("DisplayVersion").ToString();
                    UBR = WindowsRK.GetValue("UBR").ToString();
                    Version = $"{DisplayVersion} ({Build}.{UBR})";

                    if (Build >= 22000)
                    {
                        ProductName = ProductName.Replace("Windows 10", "Windows 11");
                        Logo = new Uri("pack://application:,,,/Assets/SVG/Windows/11.svg");
                        LogoPNG = new BitmapImage(new Uri("pack://application:,,,/Assets/PNG/Win11.png"));
                    }
                    else
                    {
                        Logo = new Uri("pack://application:,,,/Assets/SVG/Windows/10.svg");
                        //LogoPNG = new BitmapImage(new Uri("pack://application:,,,/Assets/PNG/Win10.png"));
                    }

                    GetMinimalInfo = $"{ProductName} {DisplayVersion} {Platform}";

                    break;
            }

            if (ProductName.Contains("Enterprise LTSB 2016"))
            {
                EditionID = "EnterpriseSB";
            }

            if (ProductName.Contains("Enterprise N LTSB 2016"))
            {
                EditionID = "EnterpriseSNB";
            }

            if (EditionID == "ServerRdsh" && Build <= 17134)
            {
                EditionID = "ServerRdsh134";
            }

            WindowsRK.Close();

            GetAllInfo = $"Microsoft {ProductName} {Platform}";

            Directory.SetCurrentDirectory(@"C:\Windows\System32");
            //ExtractLicenseStatus();
        }
    }
}

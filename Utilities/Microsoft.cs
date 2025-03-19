using Microsoft.Win32;
using SharpVectors.Converters;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PrometheusActivator.Utilities
{
    public static class WindowsHandler
    {
        private const string WindowsPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion";


        // 7 no
        private static string DisplayVersion { get; set; }
        private static string UBR { get; set; }


        public static string Build => Registry.GetValue(WindowsPath, "CurrentBuildNumber", null).ToString();

        // 7 no
        public static string ProductName { get; set; }
        public static string Platform => Environment.Is64BitOperatingSystem ? "64 bits" : "32 bits";
        public static string Version = Build;
        public static string GetMinimalInfo = $"{ProductName} {Platform}";
        public static string GetAllInfo { get; private set; }
        public static string LicenseStatus { get; private set; }
        public static string ShellOutput { get; private set; }
        public static int ProductIndex { get; private set; }
        public static int LicenseIndex { get; private set; }
        public static bool ServerEval => ProductName.Contains("Evaluation") && ProductName.Contains("Server");
        public static Uri Logo { get; set; }
        public static BitmapImage LogoPNG { get; private set; }
        public static readonly List<(string License, string Description)> Home_Licenses = new()
        {
            ("TX9XD-98N7V-6WMQ6-BX7FG-H8Q99", ""),
            ("3KHY7-WNT83-DGQKR-F7HPR-844BM", " (N)"),
            ("7HNRX-D7KGG-3K4RQ-4WPJ4-YTDFH", " (Single Language)"),
            ("PVMJN-6DFY6-9CCP6-7BKTT-D3WVR", " (Country Specific)")
        };
        public static readonly List<(string License, string Description)> Pro_Licenses = new()
        {
            ("W269N-WFGWX-YVC9B-4J6C9-T83GX", ""),
            ("MH37W-N47XK-V7XM9-C7227-GCQG9", " (N)"),
            ("NRG8B-VKK3Q-CXVCJ-9G2XF-6Q84J", " (Pro for Workstations)"),
            ("9FNHH-K3HBT-3W4TD-6383H-6XYWF", " (Pro for Workstations N)")
        };
        public static readonly List<(string License, string Description)> Education_Licenses = new()
        {
            ("NW6C2-QMPVW-D7KKK-3GKT6-VCFB2", ""),
            ("2WH4N-8QGBV-H22JP-CT43Q-MDWWJ", " (N)"),
            ("6TP4R-GNPTD-KYYHQ-7B7DP-J447Y", " (Pro)"),
            ("YVWGF-BXNMC-HTQYQ-CPQ99-66QFC", " (Pro N)"),
        };
        public static readonly List<(string License, string Description)> Enterprise_Licenses = new()
        {
            ("NPPR9-FWDCX-D2C8J-H872K-2YT43", ""),
            ("DPH2V-TTNVB-4X9Q3-TJR4H-KHJW4", " (N)"),
            ("YYVX9-NTFWV-6MDM3-9PT4T-4M68B", " (G)"),
            ("44RPN-FTY23-9VTTB-MP9BX-T84FV", " (G N)"),
            ("M7XTQ-FN8P6-TTKYV-9D4CC-J462D", " (LTSC)"),
            ("92NFX-8DJQP-P6BBQ-THF9C-7CG2H", " (N LTSC)"),
        };
        public static readonly List<(string License, string Description)> SDServer_Licenses = new()
        {
            ("TVRH6-WHNXV-R9WG3-9XRFY-MY832", " (2025 Standard)"),
            ("VDYBN-27WPP-V4HQT-9VMD4-VMK7H", " (2022 Standard)"),
            ("N69G4-B89J2-4G8F4-WWYCC-J464C", " (2019 Standard)"),
            ("WC2BQ-8NRM3-FDDYY-2BFGV-KHKQY", " (2016 Standard)"),
        };
        public static readonly List<(string License, string Description)> DCServer_Licenses = new()
        {
            ("D764K-2NDRG-47T6Q-P8T8W-YP6DF", " (2025 Datacenter)"),
            ("WX4NM-KYWYW-QJJR4-XV3QB-6VM33", " (2022 Datacenter)"),
            ("WMDGN-G9PQG-XVVXX-R3X43-63DFG", " (2019 Datacenter)"),
            ("CB7KF-BWN84-R7R2Y-793K2-8XDDG", " (2016 Datacenter)"),
        };
        public static readonly List<(string License, string Description)> Server_Licenses = new(SDServer_Licenses.Concat(DCServer_Licenses));

        public static readonly string[] Products =
        {
            "Windows Home",
            "Windows Pro",
            "Windows Education",
            "Windows Enterprise",
            "Windows Server",
        };

        public static async Task InitializeAsync()
        {
            ProductName = Registry.GetValue(WindowsPath, "ProductName", null).ToString();

            if (int.TryParse(Build, out var buildNumber) && buildNumber >= 22000)
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
            string[] products = { "Home", "Pro", "Education", "Enterprise", "Server" };
            ProductIndex = Array.FindIndex(products, p => ProductName.Contains(p));

            string[] SDlicenses = { "2025 Standard", "2022 Standard", "2019 Standard", "2019 Datacenter", "2016 Datacenter" };
            string[] DClicenses = { "2025 Datacenter", "2022 Datacenter", "2019 Datacenter", "2016 Datacenter" };
            if (ProductName.Contains("Standard"))
            {
                LicenseIndex = Array.FindIndex(SDlicenses, p => ProductName.Contains(p));
            }
            else if (ProductName.Contains("Datacenter"))
            {
                LicenseIndex = Array.FindIndex(DClicenses, p => ProductName.Contains(p));
            }

            if (!ProductName.Contains("Windows 7"))
            {
                DisplayVersion = Registry.GetValue(WindowsPath, "DisplayVersion", null).ToString();
                UBR = Registry.GetValue(WindowsPath, "UBR", null).ToString();
                Version = $"{DisplayVersion} ({Build}.{UBR})";
                GetMinimalInfo = $"{ProductName} {DisplayVersion} {Platform}";
            }
            else
            {
                Logo = new Uri("pack://application:,,,/Assets/SVG/Windows/7_color.svg");
            }




            GetAllInfo = $"Microsoft {ProductName} {Platform}";

            Directory.SetCurrentDirectory(@"C:\Windows\System32");
            //ExtractLicenseStatus();
        }
    }
}

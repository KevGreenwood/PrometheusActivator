using Microsoft.Win32;
using System.Management;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;


namespace PrometheusActivator.Utilities
{
    public static class WindowsHandler
    {
        public static RegistryKey WindowsRK = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", false);
        public static string UBR { get; set; }
        public static string Version { get; set; }
        public static string ActID { get; set; }
        
        public static string ProductKeyID = Encoding.Unicode.GetString((byte[])WindowsRK.GetValue("DigitalProductId4"), 0x08, 0x64);
        public static string ProductKey = Encoding.Unicode.GetString((byte[])WindowsRK.GetValue("DigitalProductId4"), 0x3F8, 0x80);

        public static string licenseKey { get; private set; }
        public static string ProductName = WindowsRK.GetValue("ProductName").ToString();
        public static string EditionID = WindowsRK.GetValue("EditionID").ToString();
        public static float CurrentVersion = float.Parse(WindowsRK.GetValue("CurrentVersion").ToString()) / 10f;
        public static int Build = Convert.ToInt32(WindowsRK.GetValue("CurrentBuildNumber").ToString());
        //public static string GetMinimalInfo = $"{ProductName} {Platform}";
        public static string GetAllInfo { get; private set; }
        private static string Platform = Environment.Is64BitOperatingSystem ? "64 bits" : "32 bits";
        private static string DisplayVersion { get; set; }
      
        public static Uri Logo { get; set; }
        public static BitmapImage LogoPNG { get; private set; }

        public static async Task InitializeAsync()
        {
            switch (CurrentVersion)
            {
                case 6.1f:
                    Logo = new Uri("pack://application:,,,/Assets/SVG/Windows/7.svg");
                    UBR = WindowsRK.GetValue("CSDVersion")?.ToString() ?? string.Empty;
                    Version = $"{UBR} ({Build})";
                    break;

                case 6.2f:
                    Logo = new Uri("pack://application:,,,/Assets/SVG/Windows/10.svg");
                    Version = Build.ToString();
                    break;

                case 6.3f:
                    UBR = WindowsRK.GetValue("UBR")?.ToString() ?? string.Empty;

                    if (ProductName.Contains("8.1"))
                    {
                        Logo = new Uri("pack://application:,,,/Assets/SVG/Windows/10.svg");
                        Version = $"{Build}.{UBR}";
                    }
                    else
                    {
                        DisplayVersion = WindowsRK.GetValue("DisplayVersion").ToString();
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

                        //GetMinimalInfo = $"{ProductName} {DisplayVersion} {Platform}";
                    }
                    break;
            }

            EditionID = ProductName switch
            {
                string p when p.Contains("Enterprise LTSB 2016") => "EnterpriseSB",
                string p when p.Contains("Enterprise N LTSB 2016") => "EnterpriseSNB",
                _ => EditionID
            };

            if (EditionID == "ServerRdsh" && Build <= 17134)
            {
                EditionID = "ServerRdsh134";
            }

            GetAllInfo = $"{ProductName} {Platform}";

            

            using ManagementObjectSearcher searcher = new("SELECT ID FROM SoftwareLicensingProduct WHERE (ApplicationID='55c92734-d682-4d71-983e-d6ec3f16059f' AND PartialProductKey <> NULL)");
            ActID = searcher.Get().Cast<ManagementObject>()
                                 .FirstOrDefault()?["ID"] as string ?? string.Empty;
            GetLicenseKey();
            WindowsRK.Close();
        }

        public static void GetLicenseKey()
        {
            using ManagementObjectSearcher searcher = new("SELECT OA3xOriginalProductKey FROM SoftwareLicensingService");
            licenseKey = searcher.Get().Cast<ManagementObject>()
                                 .FirstOrDefault()?["OA3xOriginalProductKey"] as string ?? string.Empty;


            if (string.IsNullOrWhiteSpace(licenseKey) && WindowsRK != null)
            {
                switch (CurrentVersion)
                {
                    case 6.1f:
                        MessageBox.Show(KeyFinder.GetWindowsKey61(), "l0ol", MessageBoxButton.OK);
                        break;
                    case 6.2f:
                    case 6.3f:
                        if (ProductName.Contains("11"))
                        {
                            MessageBox.Show(KeyFinder.GetWindowsKey62(), "l0ol", MessageBoxButton.OK);
                        }
                        else
                        {
                            MessageBox.Show(KeyFinder.GetWindowsKey62(), "l0ol", MessageBoxButton.OK);
                        }
                        break;
                }
            }
        }
    }
}

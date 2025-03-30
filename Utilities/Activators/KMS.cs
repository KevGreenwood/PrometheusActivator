using PrometheusActivator.Utilities.Activators.Keys;
using System.Management;


namespace PrometheusActivator.Utilities.Activators
{
    public static class KMS
    {
        public static (string License, string Edition) key { get; private set; }

        public static List<string> Servers = new()
        {
            "Auto",
            "Add a custom server",
            "kms.03k.org",
            "kms-default.cangshui.net",
            "kms.sixyin.com",
            "kms.cgtsoft.com",
            "kms.idina.cn",
            "kms.moeyuuko.com",
            "xincheng213618.cn",
            "kms.loli.best",
            "kms.myds.cloud",
            "kms.0t.net.cn",
            "kms.wxlost.com",
            "kms.moeyuuko.top",
            "kms.ghpym.com",
            "kms.chinancce.com",
            "kms.ddns.net",
            "dimanyakms.sytes.net",
            "kms.digiboy.ir",
            "ms8.us.to",
            "s8.uk.to",
            "s9.us.to",
            "kms9.msguides.com",
            "kms8.msguides.com",
        };

        public static void SelectLicense()
        {
            Dictionary<float, List<(string License, string Edition)>> licenseMap = new()
            {
                { 6.1f, WindowsLicenses.Windows7 },
                { 6.2f, WindowsLicenses.Windows8 },
                { 6.3f, WindowsHandler.UBR == string.Empty ? WindowsLicenses.Windows81 : null }
            };

            if (licenseMap.TryGetValue(WindowsHandler.CurrentVersion, out List<(string License, string Edition)>? licenses) && licenses != null)
            {
                key = licenses.FirstOrDefault(x => x.Edition.Equals(WindowsHandler.EditionID, StringComparison.OrdinalIgnoreCase));
                return;
            }

            if (WindowsHandler.CurrentVersion == 6.3f)
            {
                if (!WindowsHandler.ProductName.Contains("Server"))
                {
                    key = WindowsLicenses.WindowsX.FirstOrDefault(x => x.Edition.Equals(WindowsHandler.EditionID, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    SelectServerLicense();
                }
            }
        }

        private static void SelectServerLicense()
        {
            Dictionary<string, List<(string License, string Edition)>> serverMap = new()
            {
                { "2025", WindowsLicenses.Server25 },
                { "2022", WindowsLicenses.Server22 },
                { "2019", WindowsLicenses.Server19 },
                { "2016", WindowsLicenses.Server16 }
            };
            foreach (KeyValuePair<string, List<(string License, string Edition)>> entry in serverMap.Where(entry => WindowsHandler.ProductName.Contains(entry.Key)))
            {
                key = entry.Value.FirstOrDefault(x => x.Edition.Equals(WindowsHandler.EditionID, StringComparison.OrdinalIgnoreCase));
                break;
            }

            Dictionary<string, List<(string License, string Edition)>> ubrMap = new()
            {
                { "1809", WindowsLicenses.Server1809 },
                { "1803", WindowsLicenses.Server1803 },
                { "1709", WindowsLicenses.Server1709 }
            };
            foreach (var entry in ubrMap.Where(entry => WindowsHandler.UBR.Contains(entry.Key)))
            {
                key = entry.Value.FirstOrDefault(x => x.Edition.Equals(WindowsHandler.EditionID, StringComparison.OrdinalIgnoreCase));
                break;
            }
        } 
    }
}

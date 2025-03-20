using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrometheusActivator.Utilities.Activators
{
    public class WindowsLicenses
    {
        public static readonly List<(string License, string Description)> WindowsX = new()
        {
            ("TX9XD-98N7V-6WMQ6-BX7FG-H8Q99", "Core"),
            ("3KHY7-WNT83-DGQKR-F7HPR-844BM", "CoreN"),   
            ("PVMJN-6DFY6-9CCP6-7BKTT-D3WVR", "CoreCountrySpecific"),
            ("7HNRX-D7KGG-3K4RQ-4WPJ4-YTDFH", "CoreSingleLanguage"),

            ("W269N-WFGWX-YVC9B-4J6C9-T83GX", "Professional"),
            ("MH37W-N47XK-V7XM9-C7227-GCQG9", "ProfessionalN"),
            ("NRG8B-VKK3Q-CXVCJ-9G2XF-6Q84J", "ProfessionalWorkstation"),
            ("9FNHH-K3HBT-3W4TD-6383H-6XYWF", "ProfessionalWorkstationN"),
   
            ("NW6C2-QMPVW-D7KKK-3GKT6-VCFB2", "Education"),
            ("2WH4N-8QGBV-H22JP-CT43Q-MDWWJ", "EducationN"),
            ("6TP4R-GNPTD-KYYHQ-7B7DP-J447Y", "ProfessionalEducation"),
            ("YVWGF-BXNMC-HTQYQ-CPQ99-66QFC", "ProfessionalEducationN"),

            ("NPPR9-FWDCX-D2C8J-H872K-2YT43", "Enterprise"),
            ("DPH2V-TTNVB-4X9Q3-TJR4H-KHJW4", "EnterpriseN"),
            ("YYVX9-NTFWV-6MDM3-9PT4T-4M68B", "EnterpriseG"),
            ("44RPN-FTY23-9VTTB-MP9BX-T84FV", "EnterpriseGN"),
            // LTSC 2019 - 2024
            ("M7XTQ-FN8P6-TTKYV-9D4CC-J462D", "EnterpriseS"),
            ("92NFX-8DJQP-P6BBQ-THF9C-7CG2H", "EnterpriseSN"),
            //LTSB 2016
            ("DCPHK-NFMTC-H88MJ-PFHPY-QJ4BJ", "EnterpriseSB"),
            ("QFFDN-GRT3P-VKWWX-X7T3R-8B639", "EnterpriseSNB"),
            // IoT 2021 - 2024
            ("MNXKQ-WY2CT-JWBJ2-T68TQ-YBH2V", "IoTEnterprise"),
            ("KBN8V-HFGQ4-MGXVD-347P6-PDQGT", "IoTEnterpriseS"),
            ("CPWHC-NT2C7-VYW78-DHDB2-PG3GK", "ServerRdsh"),
            ("7NBT4-WGBQX-MP4H7-QXFF8-YP3KX", "ServerRdsh134"),

            ("NBTWJ-3DR69-3C4V8-C26MC-GQ9M6", "CloudE"),
            ("37D7F-N49CB-WQR8W-TBJ73-FM8RX", "CloudEdition"),
            ("6XN7V-PCBDC-BDBRH-8DQY7-G6R44", "CloudEditionN")
        };

        public static readonly List<(string License, string Description)> Server25 = new()
        {
            ("TVRH6-WHNXV-R9WG3-9XRFY-MY832", "ServerStandard"),
            ("D764K-2NDRG-47T6Q-P8T8W-YP6DF", "ServerDatacenter"),
            ("FCNV3-279Q9-BQB46-FTKXX-9HPRH", "ServerAzureCor"),
            ("XGN3F-F394H-FD2MY-PP6FD-8MCRC", "ServerTurbine")
        };

        public static readonly List<(string License, string Description)> Server22 = new()
        {
            ("VDYBN-27WPP-V4HQT-9VMD4-VMK7H", "ServerStandard"),
            ("WX4NM-KYWYW-QJJR4-XV3QB-6VM33", "ServerDatacenter"), // AKA 23H2
            ("6N379-GGTMK-23C6M-XVVTC-CKFRQ", "ServerAzureCor"),
            ("NTBV8-9K7Q8-V27C6-M2BTV-KHMXV", "ServerTurbine")
        };
        public static readonly List<(string License, string Description)> Server19 = new()
        {
            ("N69G4-B89J2-4G8F4-WWYCC-J464C", "ServerStandard"),
            ("WMDGN-G9PQG-XVVXX-R3X43-63DFG", "ServerDatacenter"),
            ("FDNH6-VW9RW-BXPJ7-4XTYG-239TB", "ServerAzureCor"),
            ("WVDHN-86M7X-466P6-VHXV7-YY726", "ServerSolution"),          
            ("GRFBW-QNDC4-6QBHG-CCK3B-2PR88", "ServerARM64")
        };
        public static readonly List<(string License, string Description)> Server16 = new()
        {
            ("WC2BQ-8NRM3-FDDYY-2BFGV-KHKQY", "ServerStandard"),
            ("CB7KF-BWN84-R7R2Y-793K2-8XDDG", "ServerDatacenter"),
            ("VP34G-4NPPG-79JTQ-864T4-R3MQX", "ServerAzureCor"),
            ("JCKRF-N37P4-C2D82-9YXRT-4M63B", "ServerSolution"),
            ("K9FYF-G6NCK-73M32-XMVPY-F9DRR", "ServerARM64"),
            ("QN4C6-GBJD2-FB422-GHWJK-GJG2R", "ServerCloudStorage")
        };
    }
}

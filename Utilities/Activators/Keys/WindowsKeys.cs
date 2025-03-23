namespace PrometheusActivator.Utilities.Activators.Keys
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

        public static readonly List<(string License, string Description)> Server1809 = new()
        {
            ("N2KJX-J94YW-TQVFB-DG9YT-724CC", "ServerStandardACor"),
            ("6NMRW-2C8FM-D24W7-TQWMY-CWH2D", "ServerDatacenterACor"),
        };

        public static readonly List<(string License, string Description)> Server1803 = new()
        {
            ("PTXN8-JFHJM-4WC78-MPCBR-9W4KR", "ServerStandardACor"),
            ("2HXDN-KRXHB-GPYC7-YCKFJ-7FVDG", "ServerDatacenterACor"),
        };

        public static readonly List<(string License, string Description)> Server1709 = new()
        {
            ("DPCNP-XQFKJ-BJF7R-FRC8D-GF6G4", "ServerStandardACor"),
            ("6Y6KB-N82V8-D8CQV-23MJW-BWTG6", "ServerDatacenterACor"),
        };


        public static readonly List<(string License, string Description)> Windows81 = new()
        {
            ("M9Q9P-WNJJT-6PXPY-DWX8H-6XWKK", "Core"),
            ("7B9N3-D94CG-YTVHR-QBPX3-RJP64", "CoreN"),
            ("NCTT7-2RGK8-WMHRF-RY7YQ-JTXG3", "CoreCountrySpecific"),
            ("BB6NG-PQ82V-VRDPW-8XVD2-V8P66", "CoreSingleLanguage"),
            ("XYTND-K6QKT-K2MRH-66RTM-43JKP", "CoreARM"),
            ("3PY8R-QHNP9-W7XQD-G6DPH-3J2C9", "CoreConnected"),
            ("Q6HTR-N24GM-PMJFP-69CD8-2GXKR", "CoreConnectedN"),
            ("KF37N-VDV38-GRRTV-XH8X6-6F3BB", "CoreConnectedSingleLanguage"),
            ("R962J-37N87-9VVK2-WJ74P-XTMHR", "CoreConnectedCountrySpecific"),

            ("GCRJD-8NW9H-F2CDX-CCM8D-9D6T9", "Professional"),
            ("HMCNV-VVBFX-7HMBH-CTY9B-B4FXY", "ProfessionalN"),
            ("789NJ-TQK6T-6XTH8-J39CJ-J8D3P", "ProfessionalWMC"),
            ("MX3RK-9HNGX-K3QKC-6PJ3F-W8D7B", "ProfessionalStudent"),
            ("TNFGH-2R6PB-8XM3K-QYHX2-J4296", "ProfessionalStudentN"),

            ("MHF9N-XY6XB-WVXMC-BTDCT-MKKG7", "Enterprise"),
            ("TT4HM-HN7YT-62K67-RGRQJ-JFFXW", "EnterpriseN"),

            ("NMMPB-38DD4-R2823-62W8D-VXKJB", "EmbeddedIndustry"),        
            ("VHXM3-NR6FT-RY6RT-CK882-KW2CJ", "EmbeddedIndustryA"),
            ("FNFKF-PWTVT-9RC8H-32HB2-JB34X", "EmbeddedIndustryE")
        };

        public static readonly List<(string License, string Description)> Server12R2 = new()
        {
            ("D2N9P-3P6X9-2R39C-7RTCD-MDVJX", "ServerStandard"),
            ("W3GGN-FT8W3-Y4M27-J84CP-Q3VJ9", "ServerDatacenter"),
            ("KNC87-3J2TX-XB4WP-VCPJV-M4FWM", "ServerSolution"),
            ("3NPTF-33KPT-GGBPR-YX76B-39KDD", "ServerCloudStorage")
        };

        public static readonly List<(string License, string Description)> Windows8 = new()
        {
            ("BN3D2-R7TKB-3YPBD-8DRP2-27GG4", "Core"),
            ("8N2M2-HWPGY-7PGT9-HGDD8-GVGGY", "CoreN"),
            ("4K36P-JN4VD-GDC6V-KDT89-DYFKP", "CoreCountrySpecific"),
            ("2WN2H-YGCQR-KFX6K-CD6TF-84YXQ", "CoreSingleLanguage"),
            ("DXHJF-N9KQX-MFPVR-GHGQK-Y7RKV", "CoreARM"),
           
            ("NG4HW-VH26C-733KW-K6F98-J8CK4", "Professional"),
            ("XCVCF-2NXM9-723PB-MHCB7-2RYQQ", "ProfessionalN"),
            ("GNBB8-YVD74-QJHX6-27H4K-8QHDG", "ProfessionalWMC"),

            ("32JNW-9KQ84-P47T8-D8GGY-CWCK7", "Enterprise"),
            ("JMNMF-RHW7P-DMY6X-RF3DR-X2BQT", "EnterpriseN"),

            ("RYXVT-BNQG7-VD29F-DBMRY-HT73M", "EmbeddedIndustry"),
            ("NKB3R-R2F8T-3XCDP-7Q2KW-XWYQ2", "EmbeddedIndustryE")
        };

        public static readonly List<(string License, string Description)> Server12 = new()
        {
            ("XC9B7-NBPP2-83J2H-RHMBY-92BT4", "ServerStandard"),
            ("48HP8-DN98B-MYWDG-T2DCC-8W83P", "ServerDatacenter"),
            ("HTDQM-NBMMG-KGYDT-2DTKT-J2MPV", "ServerSolution"),
            ("HM7DN-YVMH3-46JC3-XYTG7-CYQJJ", "ServerMultiPointStandard"),
            ("XNH6W-2V9GX-RGJ4K-Y8X6F-QGJ2G", "ServerMultiPointPremium")
        };

        public static readonly List<(string License, string Description)> Windows7 = new()
        {
            ("FJ82H-XT6CR-J8D7P-XQJJ2-GPDD4", "Professional"),
            ("MRPKT-YTG23-K7D7T-X2JMM-QY7MG", "ProfessionalN"),
            ("W82YF-2Q76Y-63HXB-FGJG9-GF7QX", "ProfessionalE"),

            ("33PXH-7Y6KF-2VJC9-XBBR8-HVTHH", "Enterprise"),
            ("YDRBP-3D83W-TY26F-D46B2-XCKRJ", "EnterpriseN"),
            ("C29WB-22CC8-VJ326-GHFJW-H9DH4", "EnterpriseE"),

            ("YBYF6-BHCR3-JPKRB-CDW7B-F9BK4", "Embedded_POSReady"),
            ("73KQT-CD9G6-K7TQG-66MRP-CQ22C", "Embedded_ThinPC")
        };

        public static readonly List<(string License, string Description)> Server8R2 = new()
        {
            ("YC6KT-GKW9T-YTKYR-T4X34-R7VHC", "ServerStandard"),
            ("74YFP-3QFB3-KQT8W-PMXWJ-7M648", "ServerDatacenter"),
            ("489J6-VHDMP-X63PK-3K798-CPX3Y", "ServerEnterprise"),
            ("74YFP-3QFB3-KQT8W-PMXWJ-7M648", "ServerDatacenterCore"),
            ("YC6KT-GKW9T-YTKYR-T4X34-R7VHC", "ServerStandardCore"),
            ("489J6-VHDMP-X63PK-3K798-CPX3Y", "ServerEnterpriseCore"),
            ("GT63C-RJFQ3-4GMB6-BRFB9-CB83V", "ServerEnterpriseIA64"),
            ("6TPJF-RBVHG-WBW2R-86QPH-6RTM4", "ServerWeb"),
            ("TT8MH-CG224-D3D7Q-498W2-9QCTX", "ServerHPC"),
            ("6TPJF-RBVHG-WBW2R-86QPH-6RTM4", "ServerWebCore"),
            ("736RG-XDKJK-V34PF-BHK87-J6X3K", "ServerEmbeddedSolution")
        };
    }
}

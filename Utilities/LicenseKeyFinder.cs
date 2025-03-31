using System;
using System.Collections;
using Microsoft.Win32;


namespace PrometheusActivator.Utilities
{
    // Based on https://github.com/guilhermelim/Get-Windows-Product-Key
    public static class LicenseKeyFinder
    {
        private static byte[] productKeyID = (byte[])WindowsHandler.WindowsRK.GetValue("DigitalProductId");
        private const string Digits = "BCDFGHJKMPQRTVWXY2346789";
        private const int keyOffset = 52;
        private const int decodeLength = 29;
      
        // For NT 6.2 + (Windows 8 and above)
        public static string GetWindowsKey62()
        {
            string key = String.Empty;
            productKeyID[66] &= 0xf7;

            int last = 0;
            for (int i = 24; i >= 0; i--)
            {
                int current = 0;
                for (int j = 14; j >= 0; j--)
                {
                    current = productKeyID[j + keyOffset] + (current << 8);
                    productKeyID[j + keyOffset] = (byte)(current / 24);
                    current %= 24;
                    last = current;
                }
                key = Digits[current] + key;
            }

            string keypart1 = key.Substring(1, last);
            string keypart2 = key.Substring(last + 1, key.Length - (last + 1));
            key = keypart1 + "N" + keypart2;

            for (int i = 5; i < key.Length; i += 6)
            {
                key = key.Insert(i, "-");
            }

            return key;
        }

        // For NT 6.1 (Windows 7 and lower)
        public static string GetWindowsKey61()
        {
            const int keyEndIndex = keyOffset + 15;
            var decodedChars = new char[decodeLength];
            var hexPid = new ArrayList();
            for (var i = keyOffset; i <= keyEndIndex; i++)
            {
                hexPid.Add(productKeyID[i]);
            }
            for (var i = decodeLength - 1; i >= 0; i--)
            {
                if ((i + 1) % 6 != 0)
                {
                    var current = 0;
                    for (var j = 14; j >= 0; j--)
                    {
                        current = (current << 8) | (byte)hexPid[j];
                        hexPid[j] = (byte)(current / 24);
                        current %= 24;
                        decodedChars[i] = Digits[current];
                    }
                }
                else
                {
                    decodedChars[i] = '-';
                }
            }
            return new string(decodedChars);
        }
    }
}

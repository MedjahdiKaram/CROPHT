using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OphtaActivation
{
    public static class Security
    {
        public static string ToMd5(string txtToHash)
        {

            if ((txtToHash == null) || (txtToHash.Length == 0))
            {
                return string.Empty;
            }
            MD5 md5 = new MD5CryptoServiceProvider();
            var textToHash = Encoding.Default.GetBytes(txtToHash);
            var result = md5.ComputeHash(textToHash);
            return System.BitConverter.ToString(result);
        }

        public static string GetMacAddress()
        {
            return (from nic in NetworkInterface.GetAllNetworkInterfaces() where nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet && nic.OperationalStatus == OperationalStatus.Up select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
        }

    }
}

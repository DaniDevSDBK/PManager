using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PManager.Utils
{
    public class SecurityService
    {
        private static RSACryptoServiceProvider rsa;

        public static void GenerateKeys()
        {
            rsa = new RSACryptoServiceProvider();
        }

        public static string GetPublicKey()
        {
            return rsa.ToXmlString(false);
        }

        public static string Encrypt(string data)
        {
            byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(data), true);
            return Convert.ToBase64String(encryptedData);
        }

        public static string Decrypt(string encryptedData)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
            byte[] decryptedData = rsa.Decrypt(encryptedBytes, true);
            return Encoding.UTF8.GetString(decryptedData);
        }
    }
}

using PManager.Repositorios;
using System;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace PManager.Utils
{
    public class SecurityService
    {
        private static RSACryptoServiceProvider rsa;
        private static UserRepo userRepo;
        private static readonly string publicKey = "<clave pública>";

        public static void GenerateKeys()
        {
            rsa = new RSACryptoServiceProvider();
            userRepo = new UserRepo();
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

        public static string GetUserPrivateKey(int userId)
        {
            string privateKey = string.Empty;

            userRepo.GetUserPrivateKey(userId);

            return privateKey;
        }
    }
}

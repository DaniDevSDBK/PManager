using Microsoft.VisualBasic.ApplicationServices;
using PManager.Repositorios;
using System;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace PManager.Utils
{
    using System;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Security.Cryptography.Xml;
    using System.Text;

    /// <summary>
    /// Clase que proporciona funcionalidad de seguridad, incluyendo generación y gestión de claves RSA, encriptación y desencriptación de datos.
    /// </summary>
    public class SecurityService
    {
        private static RSACryptoServiceProvider rsa;
        private static UserRepo userRepo;
        private static UserContext uc = UserContext.Instance;
        private static readonly string publicKey = "<RSAKeyValue><Modulus>wS80aHnSliUoAxtZOuH3beicDsDxCkhPTyQsOCOBbyNgrlsdSL11nyQ2d5cir4vQ2rjkbKkeQesEUE2dCKykeoAwj0QKgHW+bren4Jggjp+zfJ0IS/idBJCt+ata9XiwhNbDbqeeV+9uUQdI/12wdlREpiETxMHH1R3T82vvq4Wn424s/kR2B1Pu4pInHQ5mWL+zQmnjpqSLDSmvM5woPvsa5YSB6W4jrAoo9KLtbrppgGhLVFos7mMtVb8YxxGJst7OfBXZtq/ZP5xzE74/lmZ27Y/GlEZX732kaxTXgSFxYQCUOROKNOSIxSoQebqHs8lEGzEjvphtQUaOr81WUQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        /// <summary>
        /// Genera las claves pública y privada RSA y las almacena en la base de datos si no existen.
        /// </summary>
        public static void GenerateKeys()
        {
            rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            userRepo = new UserRepo();

            string privateKey = userRepo.GetUserPrivateKey(uc.CurrentUser.Id);

            if (string.IsNullOrEmpty(privateKey))
            {
                // Genera una nueva clave privada
                privateKey = GeneratePrivateKey();

                // Almacena la clave privada en la base de datos asociada al usuario actual
                userRepo.InsertOrUpdateUserPrivateKey(uc.CurrentUser.Id, privateKey);
            }
            else
            {
                // Recupera la clave privada existente desde la base de datos
                rsa.FromXmlString(privateKey);
            }
        }

        /// <summary>
        /// Obtiene la clave pública RSA.
        /// </summary>
        /// <returns>Clave pública RSA en formato XML.</returns>
        public static string GetPublicKey()
        {
            return publicKey;
        }

        /// <summary>
        /// Encripta los datos proporcionados utilizando la clave pública RSA.
        /// </summary>
        /// <param name="data">Datos a encriptar.</param>
        /// <returns>Los datos encriptados en formato Base64.</returns>
        public static string Encrypt(string data)
        {
            byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(data), true);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Desencripta los datos encriptados utilizando la clave privada RSA.
        /// </summary>
        /// <param name="encryptedData">Datos encriptados en formato Base64.</param>
        /// <returns>Los datos desencriptados.</returns>
        public static string Decrypt(string encryptedData)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
            byte[] decryptedData = rsa.Decrypt(encryptedBytes, true);
            return Encoding.UTF8.GetString(decryptedData);
        }

        /// <summary>
        /// Obtiene la clave privada RSA asociada al usuario especificado.
        /// </summary>
        /// <param name="userId">Identificador del usuario.</param>
        /// <returns>La clave privada RSA en formato XML.</returns>
        public static string GetUserPrivateKey(int userId)
        {
            string privateKey = userRepo.GetUserPrivateKey(userId);

            return privateKey;
        }

        private static string GeneratePrivateKey()
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                // Genera una nueva clave privada de 2048 bits
                rsa.KeySize = 2048;

                // Exporta la clave privada en formato XML
                string privateKey = rsa.ToXmlString(true);

                return privateKey;
            }
        }
    }
}


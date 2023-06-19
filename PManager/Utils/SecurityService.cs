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
                //privateKey = rsa.ToXmlString(true);
                privateKey = "<RSAKeyValue><Modulus>yyUG6hZxvBNa8ax+CHiTso3IAz/uryCwaoG7PRfx9JzmlW39tJ//fVjj3hGsvW/JcmNz2kgp4qPvvhWmuzUrxu0Ty9ItleMo6ceSEND+SHxiEIlyfZULRYgLugEAXm0P6ceY3TSlxyRXNjzaqeLeQRtaJSZdXscf7fUZ8nV0Jd0=</Modulus><Exponent>AQAB</Exponent><P>9rqfVsM95jzNjakE1K//lmi8B4QqLoQX0s1hH++VRhW139S4Y6klRf8ELFI+atDmaU7fWWr77DCREnUA7hAXOw==</P><Q>0scmiEm3EtvJwjiT0TEu1mcNSm9aNl8fEAvOTKb5zHA5a+fS893hY5/fizYeDXAKMffjfZJEw75fjsQgvPrVxw==</Q><DP>elavC2ZkGvWrNsLIEm3yXbIxCckO4WG+LliIAD3b1pSNSh9ADqqgQMTiXNeq+2v50924Aa56m/K4/F3nyCNSYQ==</DP><DQ>CaETf5JujKwB0Z+oERyAGUdn8giYRHegAamoaRQPwWk3Fljm6EEwtM5u9fso8FA4BwReHjR6c77Uur73B+slrQ==</DQ><InverseQ>WQoTHHVp4eyVf7n60ezLkk6OYNXMRYSxbAz4MVbDhuOws+ge9XOAgyX8n+sZ0kKvXnPkY/vlU0DKqaPzWFTbZA==</InverseQ><D>WzIzKXGL+3/A6w+hwNHIlHbb3MGduFb3e8jjsqiGQWcSiPDI3YaaXr5CBZZvstd1WvnUr6CH1Sv9W5tCr3ZOQQEX873S3wpU64C54yiPtvxYkcoTIXEB+pkk2FuKKMEfgiiKnuGihYxSJRGfAgwjtVjZ2t+BaeDFaGR4X6MOm60=</D></RSAKeyValue>";

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
    }
}


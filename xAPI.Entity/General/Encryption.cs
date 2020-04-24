using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace xAPI.Entity.General
{
    /// <summary>
    /// Clase Encriptacion y Desencriptacion.
    /// </summary>
    public static class Encryption
    {
        private static dynamic parameters = null;

        #region Encrypt
        /// <summary>
        /// Metodo de Encriptacion que contiene un Key por Default
        /// </summary>
        /// <param name="plainText">Texto a Encriptar</param>
        /// <returns>Texto encriptado</returns>
        public static string Encrypt(string plainText)
        {
            try
            {
                string passPhrase ="@1B2c3D4e5F6g7H8x1cr3t22";
                string saltValue ="@1B2c3D4e5F6g7H8";
                string hashAlgorithm = "SHA1";

                int passwordIterations = 2;
                string initVector = "@1B2c3D4e5F6g7H8";
                int keySize = 256;

                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);


                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

                byte[] keyBytes = password.GetBytes(keySize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();

                symmetricKey.Mode = CipherMode.CBC;

                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] cipherTextBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                string cipherText = Convert.ToBase64String(cipherTextBytes);
                return cipherText;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        /// <summary>
        /// Metodo de Encriptacion que se tiene que mandar la Key para Encriptar
        /// </summary>
        /// <param name="C_ENCRYPTION_KEY"></param>
        /// <param name="C_ENCRYPTION_SALTVALUE"></param>
        /// <param name="plainText">Texto a Encriptar</param>
        /// <returns>Texto Encriptado</returns>
        public static string Encrypt(string C_ENCRYPTION_KEY, string C_ENCRYPTION_SALTVALUE, string plainText)
        {

            try
            {
                string passPhrase = C_ENCRYPTION_KEY;
                string saltValue = C_ENCRYPTION_SALTVALUE;
                string hashAlgorithm = "SHA1";

                int passwordIterations = 2;
                string initVector = "@1B2c3D4e5F6g7H8";
                int keySize = 256;

                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);


                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

                byte[] keyBytes = password.GetBytes(keySize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();

                symmetricKey.Mode = CipherMode.CBC;

                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] cipherTextBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                string cipherText = Convert.ToBase64String(cipherTextBytes);
                return cipherText;
            }
            catch (Exception )
            {
                return "";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="C_ENCRYPTION_KEY"></param>
        /// <param name="C_ENCRYPTION_SALTVALUE"></param>
        /// <param name="C_ENCRYPTION_INITVECTOR"></param>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Encrypt(string C_ENCRYPTION_KEY, string C_ENCRYPTION_SALTVALUE, string C_ENCRYPTION_INITVECTOR, string plainText)
        {

            try
            {
                string passPhrase = C_ENCRYPTION_KEY;
                string saltValue = C_ENCRYPTION_SALTVALUE;
                string hashAlgorithm = "SHA1";

                int passwordIterations = 2;
                string initVector = C_ENCRYPTION_INITVECTOR;
                int keySize = 256;

                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);


                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

                byte[] keyBytes = password.GetBytes(keySize / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();

                symmetricKey.Mode = CipherMode.CBC;

                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] cipherTextBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                string cipherText = Convert.ToBase64String(cipherTextBytes);
                return cipherText;
            }
            catch (Exception)
            {
                return "";
            }
        }
        #endregion
        #region Decrypt
        /// <summary>
        /// Metodo de Desencriptacion que contiene un Key por Default
        /// </summary>
        /// <param name="cipherText">Texto a Encriptar</param>
        /// <returns>Texto Desencriptado</returns>
        public static string Decrypt(string cipherText)
        {
            try
            {
                string passPhrase = "@1B2c3D4e5F6g7H8x1cr3t22";
                string saltValue = "@1B2c3D4e5F6g7H8";
                string hashAlgorithm = "SHA1";

                int passwordIterations = 2;
                string initVector = "@1B2c3D4e5F6g7H8";
                int keySize = 256;
                // Convert strings defining encryption key characteristics into byte
                // arrays. Let us assume that strings only contain ASCII codes.
                // If strings include Unicode characters, use Unicode, UTF7, or UTF8
                // encoding.
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

                // Convert our ciphertext into a byte array.
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

                // First, we must create a password, from which the key will be 
                // derived. This password will be generated from the specified 
                // passphrase and salt value. The password will be created using
                // the specified hash algorithm. Password creation can be done in
                // several iterations.
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

                // Use the password to generate pseudo-random bytes for the encryption
                // key. Specify the size of the key in bytes (instead of bits).
                byte[] keyBytes = password.GetBytes(keySize / 8);

                // Create uninitialized Rijndael encryption object.
                RijndaelManaged symmetricKey = new RijndaelManaged();

                // It is reasonable to set encryption mode to Cipher Block Chaining
                // (CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;

                // Generate decryptor from the existing key bytes and initialization 
                // vector. Key size will be defined based on the number of the key 
                // bytes.
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

                // Define cryptographic stream (always use Read mode for encryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                // Since at this point we don't know what the size of decrypted data
                // will be, allocate the buffer long enough to hold ciphertext;
                // plaintext is never longer than ciphertext.
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                // Start decrypting.
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                // Convert decrypted data into a string. 
                // Let us assume that the original plaintext string was UTF8-encoded.
                string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

                // Return decrypted string.   
                return plainText;
            }
            catch (Exception ex)
            { 
                return "";
            }
        }
        /// <summary>
        /// Metodo para Desencriptacion que se tiene que mandar la Key  çpara desencriptar
        /// </summary>
        /// <param name="C_ENCRYPTION_KEY"></param>
        /// <param name="C_ENCRYPTION_SALTVALUE"></param>
        /// <param name="cipherText">Texto a Encriptar</param>
        /// <returns>Texto Desencriptado</returns>
        public static string Decrypt(string C_ENCRYPTION_KEY, string C_ENCRYPTION_SALTVALUE, string cipherText)
        {

            try
            {

                string passPhrase = C_ENCRYPTION_KEY;
                string saltValue = C_ENCRYPTION_SALTVALUE;
                string hashAlgorithm = "SHA1";

                int passwordIterations = 2;
                string initVector = "@1B2c3D4e5F6g7H8";
                int keySize = 256;
                // Convert strings defining encryption key characteristics into byte
                // arrays. Let us assume that strings only contain ASCII codes.
                // If strings include Unicode characters, use Unicode, UTF7, or UTF8
                // encoding.
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

                // Convert our ciphertext into a byte array.
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

                // First, we must create a password, from which the key will be 
                // derived. This password will be generated from the specified 
                // passphrase and salt value. The password will be created using
                // the specified hash algorithm. Password creation can be done in
                // several iterations.
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

                // Use the password to generate pseudo-random bytes for the encryption
                // key. Specify the size of the key in bytes (instead of bits).
                byte[] keyBytes = password.GetBytes(keySize / 8);

                // Create uninitialized Rijndael encryption object.
                RijndaelManaged symmetricKey = new RijndaelManaged();

                // It is reasonable to set encryption mode to Cipher Block Chaining
                // (CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;

                // Generate decryptor from the existing key bytes and initialization 
                // vector. Key size will be defined based on the number of the key 
                // bytes.
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

                // Define cryptographic stream (always use Read mode for encryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                // Since at this point we don't know what the size of decrypted data
                // will be, allocate the buffer long enough to hold ciphertext;
                // plaintext is never longer than ciphertext.
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                // Start decrypting.
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                // Convert decrypted data into a string. 
                // Let us assume that the original plaintext string was UTF8-encoded.
                string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

                // Return decrypted string.   
                return plainText;
            }
            catch (Exception )
            {
                return "";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="C_ENCRYPTION_KEY"></param>
        /// <param name="C_ENCRYPTION_SALTVALUE"></param>
        /// <param name="C_ENCRYPTION_INITVECTOR"></param>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static string Decrypt(string C_ENCRYPTION_KEY, string C_ENCRYPTION_SALTVALUE, string C_ENCRYPTION_INITVECTOR, string cipherText)
        {

            try
            {

                string passPhrase = C_ENCRYPTION_KEY;
                string saltValue = C_ENCRYPTION_SALTVALUE;
                string hashAlgorithm = "SHA1";

                int passwordIterations = 2;
                string initVector = C_ENCRYPTION_INITVECTOR;
                int keySize = 256;
                // Convert strings defining encryption key characteristics into byte
                // arrays. Let us assume that strings only contain ASCII codes.
                // If strings include Unicode characters, use Unicode, UTF7, or UTF8
                // encoding.
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

                // Convert our ciphertext into a byte array.
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

                // First, we must create a password, from which the key will be 
                // derived. This password will be generated from the specified 
                // passphrase and salt value. The password will be created using
                // the specified hash algorithm. Password creation can be done in
                // several iterations.
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

                // Use the password to generate pseudo-random bytes for the encryption
                // key. Specify the size of the key in bytes (instead of bits).
                byte[] keyBytes = password.GetBytes(keySize / 8);

                // Create uninitialized Rijndael encryption object.
                RijndaelManaged symmetricKey = new RijndaelManaged();

                // It is reasonable to set encryption mode to Cipher Block Chaining
                // (CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;

                // Generate decryptor from the existing key bytes and initialization 
                // vector. Key size will be defined based on the number of the key 
                // bytes.
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

                // Define cryptographic stream (always use Read mode for encryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                // Since at this point we don't know what the size of decrypted data
                // will be, allocate the buffer long enough to hold ciphertext;
                // plaintext is never longer than ciphertext.
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                // Start decrypting.
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                // Convert decrypted data into a string. 
                // Let us assume that the original plaintext string was UTF8-encoded.
                string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

                // Return decrypted string.   
                return plainText;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string Encrypt_Sha1(string s,Boolean isascii)
        {
            String text = "";

            if (!isascii)
                text = SHA1HashStringForUTF8String(s);
            else
                text = HashCode(s);
            return text;
        }

        private static string SHA1HashStringForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        /// <summary>
        /// Convert an array of bytes to a string of hex digits
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>String of hex digits</returns>
        private static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
        private static string HashCode(string str)
        {
            string rethash = "";
            try
            {

                System.Security.Cryptography.SHA1 hash = System.Security.Cryptography.SHA1.Create();
                System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
                byte[] combined = encoder.GetBytes(str);
                hash.ComputeHash(combined);
                rethash = Convert.ToBase64String(hash.Hash);
            }
            catch (Exception ex)
            {
                string strerr = "Error in HashCode : " + ex.Message;
            }
            return rethash;
        }
        #endregion

        public static void SetParameters(dynamic parameters)
        {
            Encryption.parameters = parameters;
        }
        public static dynamic GetParameters()
        {
            return Encryption.parameters;
        }
    }
}

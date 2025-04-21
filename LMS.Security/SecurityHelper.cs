// SecurityHelper.cs - Create in LMS.Security project
// DEVELOPER: [Both members] - Security implementation - [Date]
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LMS.Security
{
    public static class SecurityHelper
    {
        private static readonly byte[] _salt = Encoding.ASCII.GetBytes("LibraryManagementSystem2023");

        /// <summary>
        /// Hashes a password using PBKDF2 with SHA256
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <returns>Hashed password string</returns>
        public static string HashPassword(string password)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, _salt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Verifies a password against a hash
        /// </summary>
        /// <param name="password">The password to check</param>
        /// <param name="hashedPassword">The stored hash</param>
        /// <returns>True if password matches, false otherwise</returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            string hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }

        /// <summary>
        /// Encrypts data using AES
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <param name="key">Encryption key</param>
        /// <returns>Base64 encoded encrypted string</returns>
        public static string Encrypt(string plainText, string key)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        /// <summary>
        /// Decrypts AES encrypted data
        /// </summary>
        /// <param name="cipherText">Encrypted text (Base64)</param>
        /// <param name="key">Decryption key</param>
        /// <returns>Decrypted string</returns>
        public static string Decrypt(string cipherText, string key)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
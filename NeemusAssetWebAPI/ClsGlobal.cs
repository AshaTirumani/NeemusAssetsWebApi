using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NeemusAssetWebAPI.Helpers
{
    public class ClsGlobal
    {
        public static string EncryptionKey = "(y6er1@n$1234567";
        public static string Salt = "0001000100010001";

        public string EncryptAES(string clearText)
        {
            byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);

            using (Aes encryptor = Aes.Create())
            {
                encryptor.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                encryptor.IV = Encoding.UTF8.GetBytes(Salt);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(
                        ms,
                        encryptor.CreateEncryptor(),
                        CryptoStreamMode.Write))
                    {
                        cs.Write(
                            clearBytes,
                            0,
                            clearBytes.Length
                        );

                        cs.Close();
                    }

                    clearText =
                        Convert.ToBase64String(
                            ms.ToArray()
                        );
                }
            }

            return clearText;
        }

        public string DecryptAES(string cipherText)
        {
            cipherText =
                cipherText.Replace(" ", "+");

            byte[] cipherBytes =
                Convert.FromBase64String(
                    cipherText
                );

            using (Aes encryptor = Aes.Create())
            {
                encryptor.Key =
                    Encoding.UTF8.GetBytes(
                        EncryptionKey
                    );

                encryptor.IV =
                    Encoding.UTF8.GetBytes(
                        Salt
                    );

                using (MemoryStream ms =
                    new MemoryStream())
                {
                    using (CryptoStream cs =
                        new CryptoStream(
                            ms,
                            encryptor.CreateDecryptor(),
                            CryptoStreamMode.Write))
                    {
                        cs.Write(
                            cipherBytes,
                            0,
                            cipherBytes.Length
                        );

                        cs.Close();
                    }

                    cipherText =
                        Encoding.UTF8.GetString(
                            ms.ToArray()
                        );
                }
            }

            return cipherText;
        }
    }
}
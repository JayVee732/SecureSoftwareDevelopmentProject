using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

/*****************************************
 * File Author: Jamie Higgins
 * File Created: 17/09/2018
 * File Last Modified: 19/12/2018
 ****************************************/

namespace SecureSoftwareDevelopmentProject
{
    class Encryption
    {

        /// <summary>
        /// Encrypt the data that will be written to the text file
        /// </summary>
        /// <param name="inputList">The list imported from StoreData()</param>
        /// <param name="key">Secret key used for the symmetric algorithm</param>
        /// <param name="iv">Initialization Vector to use for the symmetric algorithm</param>
        public static void Encrypt(List<Item> inputList, byte[] key, byte[] iv)
        {
            try
            {
                using (AesManaged aesAlg = new AesManaged())
                {
                    aesAlg.Padding = PaddingMode.PKCS7;
                    aesAlg.KeySize = 128;           // in bits
                    aesAlg.Key = new byte[128 / 8]; // 16 bytes for 128 bit encryption
                    aesAlg.IV = new byte[128 / 8];  // AES needs a 16-byte IV
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    // Needs work since you can't have more than one item, I think?
                    using (FileStream fs = File.Open(Common.fileLocation, FileMode.OpenOrCreate))
                    {
                        using (CryptoStream cStream = new CryptoStream(fs, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sWriter = new StreamWriter(cStream))
                            {
                                foreach (var line in inputList)
                                {
                                    sWriter.Write($"{line.ItemName},{line.Price},{line.Date}|");
                                }
                            }
                        }
                    }
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
            }
        }

        /// <summary>
        /// Decrypt the data from the text file
        /// </summary>
        /// <param name="key">Secret key used for the symmetric algorithm</param>
        /// <param name="iv">Initialization Vector to use for the symmetric algorithm</param>
        /// <returns>String containing decrypted contents of text file</returns>
        public static string Decrypt(byte[] key, byte[] iv)
        {
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("IV");

            string textString = string.Empty;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.KeySize = 128;           // in bits
                aesAlg.Key = new byte[128 / 8]; // 16 bytes for 128 bit encryption
                aesAlg.IV = new byte[128 / 8];  // AES needs a 16-byte IV
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (FileStream reader = new FileStream(Common.fileLocation, FileMode.Open))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(reader, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            textString = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return textString;
        }
    }
}

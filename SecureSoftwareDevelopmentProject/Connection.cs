using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

/*****************************************
 * File Author: Jamie Higgins
 * File Created: 31/09/2018
 * File Last Modified: 21/12/2018
 ****************************************/

namespace SecureSoftwareDevelopmentProject
{
    class Connection
    {
        /// <summary>
        /// Get the product from the text file
        /// </summary>
        /// <returns>The full list of items from the text file "Data.txt"</returns>
        public static List<Item> StoreData()
        {
            try
            {
                CreateFile();
                string textString = string.Empty;
                List<Item> inputList = new List<Item>();

                using (Aes myAes = Aes.Create())
                {
                    myAes.Padding = PaddingMode.PKCS7;
                    myAes.KeySize = 128;           // in bits
                    myAes.Key = new byte[128 / 8]; // 16 bytes for 128 bit encryption
                    myAes.IV = new byte[128 / 8];  // AES needs a 16-byte IV
                    textString = Encryption.Decrypt(myAes.Key, myAes.IV);
                }

                // Split the text that is being read in
                string[] dataRead = textString.Split('|');
                foreach (var item in dataRead)
                {
                    // Check if it has reached the end of the line
                    if (item == string.Empty)
                    {
                        break;
                    }
                    string[] itemInfo = item.Split(',');
                    inputList.Add(new Item(itemInfo[0], Convert.ToDecimal(itemInfo[1]), Convert.ToDateTime(itemInfo[2])));
                }
                return inputList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Create the text file if it doesn't already exist
        /// </summary>
        private static void CreateFile()
        {
            if (!File.Exists(Common.fileLocation))
            {
                using (var stream = File.Create(Common.fileLocation)) { }
            }
        }
    }
}

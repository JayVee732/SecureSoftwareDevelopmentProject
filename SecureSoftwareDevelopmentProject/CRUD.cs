using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

/*****************************************
 * File Author: Jamie Higgins
 * File Created: 19/12/2018
 * File Last Modified: 19/12/2018
 ****************************************/

namespace SecureSoftwareDevelopmentProject
{
    class CRUD
    {
        private static string fileLocation = @"Data.txt";

        /// <summary>
        /// Add a new product to the List
        /// </summary>
        /// <param name="inputList">The list imported from StoreData()</param>
        /// <param name="projectObj">Product to be added</param>
        public static void AddItemToFile(List<Item> inputList, Item projectObj)
        {
            inputList.Add(projectObj);
            using (StreamWriter file = new StreamWriter(fileLocation, true))
            {
                file.WriteLine();
            }
        }

        /// <summary>
        /// Delete an item from the text file
        /// </summary>
        /// <param name="inputList">The list imported from StoreData()</param>
        public static void DeleteItem(List<Item> inputList)
        {
            int productNumber;
            Console.WriteLine("Which item would you like to delete?");

            for (int i = 0; i < inputList.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {inputList[i].ItemName}");
            }

            Console.Write("Your choice: ");
            productNumber = Convert.ToInt32(Console.ReadLine());

            // Remove item based on index
            inputList.RemoveAt(productNumber - 1);
            // Resets the variable for next use
            productNumber = 0;
            SaveChanges(inputList);
        }

        /// <summary>
        /// Update the product in the List
        /// </summary>
        /// <param name="inputList">The list imported from StoreData()</param>
        /// <param name="productIndex">(Used with updating a product) Determines the position of the product in the list to update</param>
        /// <param name="productObj">Product to be updated</param>
        public static void UpdateItemInFile(List<Item> inputList, int? productIndex, Item productObj)
        {
            for (int i = 0; i < inputList.Count; i++)
            {
                if (i == productIndex)
                {
                    // Update product based on index in List
                    inputList[i] = productObj;
                }
            }
        }

        /// <summary>
        /// Save changes made to list of products to text file
        /// </summary>
        /// <param name="inputList">The list imported from StoreData()</param>
        public static void SaveChanges(List<Item> inputList)
        {
            try
            {
                Console.WriteLine("----------------------------------------------------");
                //using (StreamWriter writer = new StreamWriter(fileLocation))
                //{
                //    foreach (var item in inputList)
                //    {
                //        writer.Write($"{item.ItemName},{item.Price},{item.Date}|");
                //    }
                //    Console.WriteLine("Changes Saved. Press any key to return to the main menu.");
                //}

                using (AesManaged myAes = new AesManaged())
                {
                    myAes.Padding = PaddingMode.PKCS7;
                    myAes.KeySize = 128;          // in bits
                    myAes.Key = new byte[128 / 8];  // 16 bytes for 128 bit encryption
                    myAes.IV = new byte[128 / 8];   // AES needs a 16-byte IV
                    Connection.EncryptData(inputList, myAes.Key, myAes.IV);
                }
                Console.WriteLine("Changes Saved. Press any key to return to the main menu.");
            }
            catch (IOException ioe)
            {
                Console.WriteLine($"Error: {ioe}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

/*****************************************
 * File Author: Jamie Higgins
 * File Created: 31/09/2018
 * File Last Modified: 19/12/2018
 ****************************************/

namespace SecureSoftwareDevelopmentProject
{
    class Connection
    {
        private static string fileLocation = @"Data.txt";

        /// <summary>
        /// Get the product from the text file
        /// </summary>
        /// <returns>The full list of items from the text file "Data.txt"</returns>
        public static List<Item> StoreData()
        {
            try
            {
                CreateFile();

                List<Item> inputList = new List<Item>();
                using (StreamReader reader = File.OpenText(fileLocation))
                {
                    string line = string.Empty;
                    if ((line = reader.ReadLine()) != null)
                    {
                        // Split the text that is being read in
                        string[] dataRead = line.Split('|');
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
                    }
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
            if (!File.Exists(fileLocation))
            {
                using (var stream = File.Create(fileLocation)) { }
            }
        }

        /// <summary>
        /// Determines whether or not to add or update a product in the List<Item> 
        /// </summary>
        /// <param name="inputList">The list imported from StoreData()</param>
        /// <param name="addOrUpdate">true = adding new item, false = updating an existing product</param>
        /// <param name="productIndex">(Used with updating a product) Determines the position of the product in the list to update</param>
        public static void AddOrUpdate(List<Item> inputList, bool addOrUpdate, int? productIndex)
        {
            bool isPriceValid = false;
            string product, price;

            try
            {
                do
                {
                    ValidateProduct(out isPriceValid, out product, out price);
                    Item productObj = new Item(product, Convert.ToDecimal(price), DateTime.Now);
                    if (isPriceValid)
                    {
                        // Add new product or update existing
                        if (addOrUpdate)
                        {
                            CRUD.AddItemToFile(inputList, productObj);
                        }
                        else
                        {
                            CRUD.UpdateItemInFile(inputList, productIndex, productObj);
                        }
                    }
                    else
                    {
                        Console.Write("Price is not valid. Please try again.");
                    }
                } while (!isPriceValid);

                CRUD.SaveChanges(inputList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Validate the product that is going to be added or updated
        /// </summary>
        /// <param name="isPriceValid">Does the price match the validation</param>
        /// <param name="product">Name of the product</param>
        /// <param name="price">price of the product</param>
        private static void ValidateProduct(out bool isPriceValid, out string product, out string price)
        {
            Console.Write("----------------------------------------------------\nWhat is the name of the product?: ");
            product = Console.ReadLine();
            Console.Write("How much is the product?: ");
            price = Console.ReadLine();
            // Validate that the input for the price is in the correct format. eg. "€12.34"
            isPriceValid = Regex.IsMatch(price, "^[0-9]+(\\.[0-9][0-9])?$");
        }

        public  static void EncryptData(List<Item> inputList)
        {
            AesManaged aesAlg = new AesManaged();
            List<Item> encryptedData = inputList;

            try
            {
                using (FileStream fs = File.Open(fileLocation, FileMode.OpenOrCreate))
                {
                    foreach (var line in encryptedData)
                    {
                        CryptoStream cStream = new CryptoStream(fs, new AesManaged().CreateEncryptor(aesAlg.Key, aesAlg.IV), CryptoStreamMode.Write);
                        StreamWriter sWriter = new StreamWriter(cStream);
                        sWriter.WriteLine(line);
                    }


                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
            }
        }
    }
}

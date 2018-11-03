using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

/*****************************************
 * File Author: Jamie Higgins
 * File Created: 31/09/2018
 * File Last Modified: 03/11/2018
 ****************************************/

namespace SecureSoftwareDevelopmentProject
{
    class Connection
    {
        /// <summary>
        /// Save changes made to list of products to text file
        /// </summary>
        /// <param name="inputList">The list imported from StoreData()</param>
        public static void SaveChanges(List<Item> inputList)
        {
            try
            {
                Console.WriteLine("----------------------------------------------------");
                using (StreamWriter writer = new StreamWriter(@"Data.txt"))
                {
                    foreach (var item in inputList)
                    {
                        writer.WriteLine($"{item.ItemName},{item.Price},{item.Date}");
                    }
                    Console.WriteLine("Changes Saved. Press any key to return to the main menu.");
                }
            }
            catch (IOException ioe)
            {
                Console.WriteLine($"Error: {ioe}");
            }
        }

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
                using (StreamReader reader = File.OpenText(@"Data.txt"))
                {
                    string line = string.Empty;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Read each line in and add it to the List
                        string[] itemInfo = line.Split(',');
                        inputList.Add(new Item(itemInfo[0], Convert.ToDecimal(itemInfo[1]), Convert.ToDateTime(itemInfo[2])));
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
            if (!File.Exists(@"Data.txt"))
            {
                using (var stream = File.Create(@"Data.txt")) { }
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
                            AddItemToFile(inputList, productObj);
                        }
                        else
                        {
                            UpdateItemInFile(inputList, productIndex, productObj);
                        }
                    }
                    else
                    {
                        Console.Write("Price is not valid. Please try again.");
                    }
                } while (!isPriceValid);

                SaveChanges(inputList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Update the product in the List
        /// </summary>
        /// <param name="inputList">The list imported from StoreData()</param>
        /// <param name="productIndex">(Used with updating a product) Determines the position of the product in the list to update</param>
        /// <param name="productObj">Product to be updated</param>
        private static void UpdateItemInFile(List<Item> inputList, int? productIndex, Item productObj)
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
        /// Add a new product to the List
        /// </summary>
        /// <param name="inputList">The list imported from StoreData()</param>
        /// <param name="projectObj">Product to be added</param>
        private static void AddItemToFile(List<Item> inputList, Item projectObj)
        {
            inputList.Add(projectObj);
            using (StreamWriter file = new StreamWriter(@"Data.txt", true))
            {
                file.WriteLine();
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
    }
}

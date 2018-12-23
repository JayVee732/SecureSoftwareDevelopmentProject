using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

/*****************************************
 * File Author: Jamie Higgins
 * File Created: 19/12/2018
 * File Last Modified: 23/12/2018
 ****************************************/

namespace SecureSoftwareDevelopmentProject
{
    class CRUD
    {
        /// <summary>
        /// Determines whether or not to add or update a product in the List<Item> 
        /// </summary>
        /// <param name="inputList">The list imported from StoreData()</param>
        /// <param name="addOrUpdate">true = adding new item, false = updating an existing product</param>
        /// <param name="productIndex">(Used with updating a product) Determines the position of the product in the list to update</param>
        internal static void AddOrUpdate(List<Item> inputList, bool addOrUpdate, int? productIndex)
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
                            inputList.Add(productObj);
                        }
                        else
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
                    }
                    else
                    {
                        Console.Write("Price is not valid. Please try again.");
                    }
                } while (!isPriceValid);

                Connection.SaveChanges(inputList);
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

        /// <summary>
        /// Delete an item from the text file
        /// </summary>
        /// <param name="inputList">The list imported from StoreData()</param>
        internal static void DeleteItem(List<Item> inputList)
        {
            if (inputList.Count != 0)
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
                Connection.SaveChanges(inputList);
            }
            else
            {
                Console.WriteLine("No items are currently in the till, try adding some.");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

/*****************************************
 * File Author: Jamie Higgins
 * File Created: 17/09/2018
 * File Last Modified: 30/10/2018
 ****************************************/

namespace SecureSoftwareDevelopmentProject
{
    class Program
    {
        /// <summary>
        /// Main method for the applcation
        /// </summary>
        static void Main()
        {
            // TODO: Reload Data
            string choice;
            List<Item> inputList = StoreData();

            Console.WriteLine("Money Machine Totals");

            do
            {
                // Main menu for the application
                Console.WriteLine("\nWhat would you like to do?:\n" +
                    "1. View All Data\n" +
                    "2. Add New Products\n" +
                    "3. Update a Product\n" +
                    "4. Delete a Product\n" +
                    "5. Save Changes\n" +
                    "6. Exit\n\n" +
                    "Your choice:");
                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAllData(inputList);
                        Console.Write("Press any key to return to the main menu");
                        break;
                    case "2":
                        AddNewProduct();
                        Console.Write("Press any key to return to the main menu");
                        break;
                    case "3":
                        UpdateProduct(inputList);
                        Console.Write("Press any key to return to the main menu");
                        break;
                    case "4":
                        DeleteItem(inputList);
                        Console.Write("Press any key to return to the main menu");
                        break;
                    case "5":
                        SaveChanges(inputList);
                        Console.Write("Press any key to exit the application.");
                        break;
                    case "6":
                        Console.Write("Press any key to exit the application.");
                        break;
                    default:
                        Console.Write("Not a valid input. Press any key to return to the main menu.\n");
                        break;
                }

                Console.ReadLine();
            } while (choice != "6"); // User exits the application
        }

        /// <summary>
        /// Add a new product to the application
        /// </summary>
        private static void AddNewProduct()
        {
            bool isPriceValid;
            Console.Write("\nWhat is the name of the product?: ");
            var product = Console.ReadLine();

            do
            {
                Console.Write("How much is the product?: ");
                var price = Console.ReadLine();
                // Validate that the input for the price is in the correct format. eg. "€12.34"
                isPriceValid = Regex.IsMatch(price, "^[0-9]+(\\.[0-9][0-9])?$");

                if (isPriceValid)
                {
                    using (StreamWriter file = new StreamWriter(@"Data.txt", true))
                    {
                        file.WriteLine($"{product},{price}");
                    }
                }
                else
                {
                    Console.Write("Price is not valid. Please try again.");
                }
            } while (!isPriceValid);
        }

        /// <summary>
        /// Display the contents of the text file to the console
        /// </summary>
        private static void ViewAllData(List<Item> inputList)
        {
            foreach (var item in inputList)
            {
                // TODO: Redo this as a table
                Console.WriteLine($"\tProduct: {item.ItemName}\n" +
                                    $"\tPrice: {item.Price}\n");
            }
        }

        public static List<Item> StoreData()
        {
            List<Item> inputList = new List<Item>();
            using (StreamReader reader = File.OpenText(@"Data.txt"))
            {
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] words = line.Split(',');
                    inputList.Add(new Item(words[0], Convert.ToDecimal(words[1])));
                }
            }

            return inputList;
        }

        /// <summary>
        /// User selects a product they would like to update
        /// </summary>
        private static void UpdateProduct(List<Item> inputList)
        {
            bool isPriceValid;
            Console.WriteLine("Which file would you like to update?");

            for (int i = 0; i < inputList.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {inputList[i].ItemName}");
            }

            var product = Console.ReadLine();

            do
            {
                Console.Write("What is the name of the product?: ");
                product = Console.ReadLine();
                Console.Write("How much is the product?: ");
                var price = Console.ReadLine();
                // Validate that the input for the price is in the correct format. eg. "€12.34"
                isPriceValid = Regex.IsMatch(price, "^[0-9]+(\\.[0-9][0-9])?$");

                if (isPriceValid)
                {
                    // TODO: Finish this
                }
                else
                {
                    Console.Write("Price is not valid. Please try again.");
                }
            } while (!isPriceValid);
        }

        private static void DeleteItem(List<Item> inputList)
        {
            int productNumber;
            Console.WriteLine("\nWhich item would you like to delete?: ");

            for (int i = 0; i < inputList.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {inputList[i].ItemName}");
            }

            productNumber = Convert.ToInt32(Console.ReadLine());

            inputList.RemoveAt(productNumber - 1);
            productNumber = 0;
            SaveChanges(inputList);
        }

        private static void SaveChanges(List<Item> inputList)
        {
            using (StreamWriter writer = new StreamWriter(@"Data.txt"))
            {
                foreach (var item in inputList)
                {
                    writer.WriteLine($"{item.ItemName},{item.Price}");
                }
                Console.WriteLine("Changes Saved. Press any key to return to the main menu.");
            }
        }
    }
}

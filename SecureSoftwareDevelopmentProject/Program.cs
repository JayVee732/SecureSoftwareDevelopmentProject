using System;
using System.Collections.Generic;

/*****************************************
 * File Author: Jamie Higgins
 * File Created: 17/09/2018
 * File Last Modified: 31/10/2018
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
            List<Item> inputList = Connection.StoreData();

            Console.WriteLine("Money Machine Totals");
            MainMenu(inputList);
        }

        private static void MainMenu(List<Item> inputList)
        {
            string choice;
            do
            {
                // Main menu for the application
                Console.Write("\nWhat would you like to do?:\n" +
                    "1. View All Data\n" +
                    "2. Add New Products\n" +
                    "3. Update a Product\n" +
                    "4. Delete a Product\n" +
                    "5. Exit\n\n" +
                    "Your choice: ");
                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAllData(inputList);
                        break;
                    case "2":
                        Connection.AddOrUpdate(inputList, true, null);
                        break;
                    case "3":
                        GetUpdateProductIndex(inputList);
                        break;
                    case "4":
                        Connection.DeleteItem(inputList);
                        break;
                    case "5":
                        Console.Write("Press any key to exit the application.");
                        break;
                    default:
                        Console.Write("Not a valid input. Press any key to return to the main menu.\n");
                        break;
                }

                Console.ReadLine();
            } while (choice != "5"); // User exits the application
        }

        /// <summary>
        /// Display the contents of the text file to the console
        /// </summary>
        /// <param name="inputList">The list imported from StoreData()</param>
        private static void ViewAllData(List<Item> inputList)
        {
            if (inputList.Count != 0)
            {
                foreach (var item in inputList)
                {
                    // TODO: Redo this as a table
                    Console.WriteLine($"\tProduct: {item.ItemName}\n" +
                                        $"\tPrice: {item.Price}\n" +
                                        $"\tDate Added: {item.Date}");
                }
                Console.WriteLine("Press any key to return to the main menu.");
            }
            else
            {
                Console.WriteLine("No items are currently in the till, try adding some.");
            }
        }

        /// <summary>
        /// User selects a product they would like to update
        /// </summary>
        /// <param name="inputList">The list imported from StoreData()</param>
        private static void GetUpdateProductIndex(List<Item> inputList)
        {
            Console.WriteLine("Which product would you like to update?");

            for (int i = 0; i < inputList.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {inputList[i].ItemName}");
            }

            Console.Write("Your choice: ");
            var productIndex = Convert.ToInt32(Console.ReadLine()) - 1;

            Connection.AddOrUpdate(inputList, false, productIndex);
        }
    }
}

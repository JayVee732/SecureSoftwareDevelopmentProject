using System;
using System.Collections.Generic;

/*****************************************
 * File Author: Jamie Higgins
 * File Created: 17/09/2018
 * File Last Modified: 19/12/2018
 ****************************************/

namespace SecureSoftwareDevelopmentProject
{
    class Program
    {
        /// <summary>
        /// Main method and entry point for the applcation
        /// </summary>
        static void Main()
        {
            // The application first grabs all of the data from the text file and stores it in a List
            List<Item> inputList = Connection.StoreData();

            Console.WriteLine("Data in Till Machine\n");
            MainMenu(inputList);
        }

        /// <summary>
        /// Displays a menu where the user and choose what option they would like to perform
        /// </summary>
        /// <param name="inputList">The list imported from StoreData()</param>
        private static void MainMenu(List<Item> inputList)
        {
            string choice;
            do
            {
                Console.Write("What would you like to do?:\n" +
                    "1. View All Data\n" +
                    "2. Add New Product\n" +
                    "3. Update a Product\n" +
                    "4. Delete a Product\n" +
                    "5. Exit\n\n" +
                    "Your choice: ");
                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("View All Data\n----------------------------------------------------");
                        ViewAllData(inputList);
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("Add New Product");
                        Connection.AddOrUpdate(inputList, true, null);
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine("Update a Product\n----------------------------------------------------");
                        GetUpdateProductIndex(inputList);
                        break;
                    case "4":
                        Console.Clear();
                        Console.WriteLine("Delete a Product\n----------------------------------------------------");
                        CRUD.DeleteItem(inputList);
                        break;
                    case "5":
                        Console.Clear();
                        Console.WriteLine("Exit\n----------------------------------------------------\nPress any key to exit the application.");
                        break;
                    default:
                        Console.Write("Not a valid input. Press any key to return to the main menu.\n");
                        break;
                }

                Console.ReadLine();
                Console.Clear();
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
                Console.WriteLine($"|{"Product",10}|{"Price",10}|{"Date",25}|\n----------------------------------------------------");
                foreach (var item in inputList)
                {
                    Console.WriteLine($"|{item.ItemName,10}|{item.Price,10}|{item.Date,25}|");
                }
                Console.WriteLine("----------------------------------------------------\nPress any key to return to the main menu.");
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

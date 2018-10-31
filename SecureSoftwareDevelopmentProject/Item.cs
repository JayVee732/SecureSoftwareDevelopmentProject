using System;

/*****************************************
 * File Author: Jamie Higgins
 * File Created: 30/09/2018
 * File Last Modified: 31/10/2018
 ****************************************/

namespace SecureSoftwareDevelopmentProject
{
    class Item
    {
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        public Item(string itemName, decimal price, DateTime date)
        {
            ItemName = itemName;
            Price = price;
            Date = date;
        }
    }
}

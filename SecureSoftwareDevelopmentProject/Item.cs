using System;

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

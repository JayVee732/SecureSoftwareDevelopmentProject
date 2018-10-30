using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureSoftwareDevelopmentProject
{
    class Item
    {
        public string ItemName { get; set; }
        public decimal Price { get; set; }

        public Item(string itemName, decimal price)
        {
            ItemName = itemName;
            Price = price;
        }
    }
}

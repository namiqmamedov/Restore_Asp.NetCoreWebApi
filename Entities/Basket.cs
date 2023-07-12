using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Basket
    {
        public int ID { get; set; }
        public string BuyerID { get; set; }
        public List<BasketItem> Items { get; set; } =  new(); // new List<BasketItem>();  ozu ozune initialize eliyerikki 
                                                             //teze tipden basket qaytarsin if same line just use new

        public void AddItem(Product product,int quantity)
        {
            if(Items.All(item=> item.ProductID != product.ID))
            {
                Items.Add(new BasketItem{Product = product, Quantity = quantity});
            }

            var existingItem = Items.FirstOrDefault(item => item.ProductID == product.ID);

            if(existingItem != null) existingItem.Quantity += quantity;    
        }

        public void RemoveItem(int productID, int quantity)
        {
            var item = Items.FirstOrDefault(item => item.ProductID == productID);
            
            if(item == null) return;

            item.Quantity -= quantity;

            if(item.Quantity == 0) Items.Remove(item);

        }
    }
}
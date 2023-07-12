namespace API.Entities
{
    public class BasketItem
    {
        public int ID { get; set; }
        public int Quantity { get; set; }


        // navigation properties
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
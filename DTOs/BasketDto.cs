using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class BasketDto
    {
        public int ID { get; set; }
        public string BuyerID { get; set; }
        public List<BasketItemDto> Items { get; set; }
        public string PaymentIntentID { get; set; }
        public string ClientSecret { get; set; }
    }
}
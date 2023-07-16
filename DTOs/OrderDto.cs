using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.OrderAggregate;

namespace API.DTOs
{
    public class OrderDto
    {
        public int ID { get; set; }
        public string BuyerID { get; set; }
        [Required]
        public ShippingAddress ShippingAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public long Subtotal { get; set; }
        public long DeliveryFee { get; set; }
        public string OrderStatus { get; set; }

        public long Total { get; set; }
    }
}
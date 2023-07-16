using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class OrderExtensions
    {
        public static IQueryable<OrderDto> ProjectOrderToOrderDto(this IQueryable<Order> query)
        {
            return query
            .Select(order => new OrderDto
            {
                ID = order.ID,
                BuyerID = order.BuyerID,
                OrderDate = order.OrderDate,
                ShippingAddress = order.ShippingAddress,
                DeliveryFee = order.DeliveryFee,
                Subtotal = order.Subtotal,
                OrderStatus = order.OrderStatus.ToString(), // because it's enum
                Total = order.GetTotal(),
                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                   ProductID = item.ItemOrdered.ProductID,
                   Name = item.ItemOrdered.Name,
                   PictureURL = item.ItemOrdered.PictureURL, 
                   Price = item.Price, 
                   Quantity = item.Quantity, 
                }).ToList()
            }).AsNoTracking(); 
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities.OrderAggregate;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Authorize]
    public class OrderController : BaseApiController
    {
        public StoreContext _context { get; }
        public OrderController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Where(x => x.BuyerID == User.Identity.Name)
                .ToListAsync(); 
        }

        [HttpGet("{id}",Name ="GetOrder")]
        
        public async Task<ActionResult<Order>> GetOrder(int id) 
        {
            return await _context.Orders
            .Include(x => x.OrderItems)
            .Where(x => x.BuyerID == User.Identity.Name && x.ID == id)
            .FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto orderDto)
        {
             var basket = await _context.Baskets
             .RetrieveBasketWithItems(User.Identity.Name)
             .FirstOrDefaultAsync();

             if(basket == null) return BadRequest(new ProblemDetails{Title = "Could not locate basket"});

             var items = new List<OrderItem>();

             foreach (var item in basket.Items)
             {
                var productItem = await _context.Products.FindAsync(item.ProductID);

                var itemOrdered = new ProductItemOrdered
                {
                    ProductID = productItem.ID,
                    Name = productItem.Name,
                    PictureURL = productItem.PictureURL
                };

                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };

                items.Add(orderItem);
                productItem.QuantityInStock -= item.Quantity;
             }

             var subtotal = items.Sum(item => item.Price * item.Quantity);
             var deliveryFee = subtotal > 10000 ? 0 : 500;

             var order = new Order
             {
                OrderItems = items,
                BuyerID = User.Identity.Name,
                ShippingAddress = orderDto.ShippingAddress,
                Subtotal = subtotal,
                DeliveryFee = deliveryFee
             };

             _context.Orders.Add(order);
             _context.Baskets.Remove(basket);

             if(orderDto.SaveAddress)
             {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
                user.Address = new UserAddress
                {
                    FullName = orderDto.ShippingAddress.FullName,
                    Address1 = orderDto.ShippingAddress.Address1,
                    Address2 = orderDto.ShippingAddress.Address2,
                    City = orderDto.ShippingAddress.City,
                    State = orderDto.ShippingAddress.State,
                    Zip = orderDto.ShippingAddress.Zip,
                    Country = orderDto.ShippingAddress.Country,
                };
                _context.Update(user);
             }

             var result = await _context.SaveChangesAsync() > 0;

             if(result) return CreatedAtRoute("GetOrder", new {id = order.ID}, order.ID);

             return BadRequest("Problem creating order");
        }

    }
}
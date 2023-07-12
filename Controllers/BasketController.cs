using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly StoreContext _context;

        public BasketController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<Basket>> GetBasket()
        {
            var basket = await RetrieveBasket();

            if (basket == null) return NotFound();

            return basket;
        }

        [HttpPost]
        public async Task<ActionResult> AddItemBasket(int productID, int quantity)
        {
           var basket = await RetrieveBasket();

            if(basket != null) basket = CreateBasket();

            var product = await _context.Products.FindAsync(productID);

            if(product == null) return NotFound(); 

            basket.AddItem(product,quantity);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return StatusCode(201);

            return BadRequest(new ProblemDetails{Title = "Problem saving item to basket"});
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveBasketItem(int productID,int quantity)
        {
            // get basket
            // remove item or reduce quantity
            // save changes

            return Ok();
        }

        private async Task<Basket> RetrieveBasket()
        {
            return await _context.Baskets
            .Include(i => i.Items)
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(x => x.BuyerID == Request.Cookies["buyerID"]);
        }

        private Basket CreateBasket()
        {
            var buyerID = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions{IsEssential = true,Expires = DateTime.Now.AddDays(30)};

            Response.Cookies.Append("buyerID", buyerID,cookieOptions);

            var basket = new Basket{BuyerID = buyerID};
            
            _context.Baskets.Add(basket);
            return basket;
        }
    }
}
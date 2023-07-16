using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
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

        [HttpGet(Name = "GetBasket")]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await RetrieveBasket(GetBuyerID());

            if (basket == null) return NotFound();

            return basket.MapBasketToDto();
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> AddItemBasket(int productID, int quantity)
        {
           var basket = await RetrieveBasket(GetBuyerID());

            if(basket == null) basket = CreateBasket();

            var product = await _context.Products.FindAsync(productID);

            if(product == null) return BadRequest(new ProblemDetails{Title = "Product not found"}); 

            basket.AddItem(product,quantity);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return CreatedAtRoute("GetBasket", basket.MapBasketToDto());

            return BadRequest(new ProblemDetails{Title = "Problem saving item to basket"});
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveBasketItem(int productID,int quantity)
        {
           var basket = await RetrieveBasket(GetBuyerID());

           if(basket == null) return NotFound();

            basket.RemoveItem(productID,quantity);

            var result = await _context.SaveChangesAsync() > 0;

            if(result) return Ok();

            return BadRequest(new ProblemDetails{Title = "Problem removing item from the basket"});
        }

        private async Task<Basket> RetrieveBasket(string buyerID)
        {
            if(string.IsNullOrEmpty(buyerID))
            {
                Response.Cookies.Delete("buyerID");
                return null;
            }

            return await _context.Baskets
            .Include(i => i.Items)
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(x => x.BuyerID == buyerID);
        }

        private string GetBuyerID()
        {
            return User.Identity?.Name ?? Request.Cookies["buyerID"]; // if dont have user get a cookie
        }

        private Basket CreateBasket()
        {
            var buyerID = User.Identity?.Name;
            if(string.IsNullOrEmpty(buyerID))
            {
                buyerID = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions{IsEssential = true,Expires = DateTime.Now.AddDays(30)};
                Response.Cookies.Append("buyerID", buyerID,cookieOptions);
            }

            var basket = new Basket{BuyerID = buyerID};
            
            _context.Baskets.Add(basket);
            return basket;
        }
    }
}
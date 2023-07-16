using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Extensions
{
    public static class BasketExtension
    {
        public static BasketDto MapBasketToDto(this Basket basket)
        {
            return new BasketDto
            {
                ID = basket.ID,
                BuyerID = basket.BuyerID,
                Items = basket.Items.Select(item => new BasketItemDto
                {
                    ProductID = item.ProductID,
                    Name = item.Product.Name,
                    Price = item.Product.Price,
                    PictureURL = item.Product.PictureURL,
                    Type = item.Product.Type,
                    Brand = item.Product.Brand,
                    Quantity = item.Quantity
                }).ToList()
            };
        }
    }
}
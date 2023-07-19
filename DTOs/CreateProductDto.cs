using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CreateProductDto
    {
       public int ID { get; set; }
       [Required]
       public string Name { get; set; } 
       [Required]
       public string Description { get; set; } 
       [Required]
       public long Price { get; set; } 
       public IFormFile PictureURL { get; set; }
       [Required]
       public string Type { get; set; }
       [Required]
       public string Brand { get; set; }
       [Required]
       [Range(0,200)]
       public int QuantityInStock { get; set; }
    }
}
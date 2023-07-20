using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UpdateProductDto
    {
       public int ID { get; set; }
       [Required]
       public string Name { get; set; } 
       [Required]
       public string Description { get; set; } 
       [Required]
       public long Price { get; set; } 
       public IFormFile File { get; set; }
       [Required]
       public string Type { get; set; }
       [Required]
       public string Brand { get; set; }
       [Required]
       [Range(0,200)]
       public int QuantityInStock { get; set; }
    }
}
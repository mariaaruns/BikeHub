using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Request
{
    public class AddProductsDto
    {
        [Required]
        public string ProductName { get; set; }
        
        [Required]
        public int BrandId { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        
        [Required]
        public int ModelYear { get; set; }
        
        [Required]
        public decimal ListPrice { get; set; }
        public IFormFile? ProductImage { get; set; }
        public string? ImageUrl { get; set; }

        public int StockQty { get; set; }
    }
}

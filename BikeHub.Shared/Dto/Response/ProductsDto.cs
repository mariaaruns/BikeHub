using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Response
{
    public class ProductsDto { 
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string CategoryName { get; set; }
            public decimal? Price {get; set; }
            public int? Stock {get; set; }
            public string? ProductImage { get; set; }
    };

}

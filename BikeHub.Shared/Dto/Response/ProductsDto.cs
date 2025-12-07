using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public class ProductDto1 
    {
      public int category_id { get; set; }
      public string? category_name { get; set; }
    }
    public class productDto2
    {
        public int product_id { get; set; }
        public string? product_name { get; set; }
        public int TotalQuantity { get; set; }
    }
}

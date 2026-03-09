using BikeHub.Shared.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Response
{
    public class ProductsDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public int BrandId { get; set; }
        public string? BrandName { get; set; }

        public string? ModelYear { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }

        //  private string? _productImage;
        public string? ProductImage { get; set; }

        public string? ProductImageUrl { get; set; }
    };



}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Response
{
    public class GetProductByIdDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public string? ProductImage { get; set; }
        public string ModelYear { get; set; }

    }
}

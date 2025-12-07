using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Response
{
    public class ProductDropdownDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int StockQty { get; set; }
        public decimal Price { get; set; }
    }
}

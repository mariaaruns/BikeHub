using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.ReportsDto
{
    public class TopProductsByRevenueDto
    {
        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public int UnitsSold { get; set; }

        public decimal Revenue { get; set; }
    }
}

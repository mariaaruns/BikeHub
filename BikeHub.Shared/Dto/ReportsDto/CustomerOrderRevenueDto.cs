using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.ReportsDto
{
    public  class CustomerOrderRevenueDto
    {
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int TotalOrders { get; set; }

        public decimal TotalRevenue { get; set; }
        
        public int TotalUnits { get; set; }

        public decimal AvgUnitPrice { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Response
{
    public class DashboardResponseDto
    {
        public int TotalProductsCount { get; set; }
        public int TotalOrdersCount { get; set; }
        public int TotalServiceCount { get; set; } 
        public int PendingServiceCount { get; set; }
        public int CompletedServiceCount { get; set; }
    }
}

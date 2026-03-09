using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Response
{
    public class SalesAmountByYearDto
    {
        public string Month { get; set; } = string.Empty;

        public decimal NetAmount { get; set; } = 0m;
    }

    public class BrandYearlySalesDto
    {
        public string Brand { get; set; } = string.Empty;
        public decimal NetAmount { get; set; } = 0m;
    }
}

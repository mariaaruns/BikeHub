using BikeHub.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Request
{
    public class GetOrderDto:PaginationBaseModel
    {
        public int? OrderId { get; set; }    
        public int? OrderStatus { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? PaymentStatus { get; set; }

    }
}

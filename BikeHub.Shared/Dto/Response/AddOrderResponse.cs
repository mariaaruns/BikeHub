using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Response
{
    public class AddOrderResponse
    { 
        public string RazorpayOrderId { get; set; }

        public long OrderId { get; set; }

        public string RazorpaySecretKey { get; set; }
    }
}

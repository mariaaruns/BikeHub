using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Enum
{
    public class Enums
    {
        public enum PaymentStatus{
            VerificationFailed = 1009,
            PaymentFailed = 1010,
            PaymentSuccess = 1011
        }

        public enum OrderStatus{
            Processing=1,
            Shipped=2,
            Cancelled=3,
            Delivered=4,
        }

        public enum ServiceStatus
        {
            Pending=1005,
            InProgress=1006,
            Completed=1007,
            Cancelled=1008
        }


    }
}

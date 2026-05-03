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
        public enum PaymentStatus
        {
            Unpaid = 1016,
            Initiated = 1017,
            Paid = 1018,
            Failed = 1019,
            Refunded = 1020
        }

        public enum PaymentMethod
        {
            Online = 15,
            Cash = 14
        }

        public enum OrderStatus
        {
            Processing = 1,
            Shipped = 2,
            Cancelled = 3,
            Delivered = 4,
        }

        public enum ServiceStatus
        {
            Pending = 1005,
            InProgress = 1006,
            Completed = 1007,
            ReadyForPickup = 1008,
            Delivered = 1012,
            Cancelled = 1013
        }


    }
}

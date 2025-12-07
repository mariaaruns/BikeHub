using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Request
{
    public class AddOrderRequest
    {

        public int CustomerId { get; set; }

        public int OrderStatus { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime RequiredDate { get; set; }

        public DateTime ShippedDate { get; set; }

        public int StaffId { get; set; }

        public List<OrderItemRequest> OrderItemRequests { get; set; } = new List<OrderItemRequest>();

    }

    public class OrderItemRequest
    {
        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
    }
}

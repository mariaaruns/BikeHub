using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Response
{
    public class OrderDetailResponse
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int StoreId { get; set; }
        public int StaffId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public List<OrderItemDetail> OrderItems { get; set; } = new List<OrderItemDetail>();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Request
{
    public class AddOrderRequest
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int OrderStatus { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime RequiredDate { get; set; }

        [Required]
        public DateTime ShippedDate { get; set; }

        [Required]
        public int StaffId { get; set; }

        //[Required]
        //public int StoreId { get; set; }

        [Required]
        public OrderItemRequest[] OrderItemRequests { get; set; } = [];

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

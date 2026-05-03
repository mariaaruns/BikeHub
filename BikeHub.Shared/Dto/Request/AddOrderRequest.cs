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

        
        public int OrderStatus { get; set; }

        
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime RequiredDate { get; set; }

        
        public DateTime ShippedDate { get; set; }

        [Required]
        public int StaffId { get; set; }


        public string RazorpayOrderId { get; set; }


        public int PaymentMethod {get;set;}
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

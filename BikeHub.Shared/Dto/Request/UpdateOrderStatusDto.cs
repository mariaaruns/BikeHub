using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Request
{
    public class UpdateOrderStatusDto
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int OrderStatusId { get; set; }  

    }
}

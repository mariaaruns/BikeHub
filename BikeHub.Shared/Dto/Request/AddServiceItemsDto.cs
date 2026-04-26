using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Request
{
    public class AddServiceItemsDto
    {
        [Required]
        public long ServiceJobId { get; set; }
        [Required]
        public int PartId { get; set; }
        [Required]
        public int Qty { get; set; } = 1;
        [Required]
        public decimal Total { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
    }
}

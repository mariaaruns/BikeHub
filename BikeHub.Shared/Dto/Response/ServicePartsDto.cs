using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Response
{
    public class ServicePartsDto
    {
        public int ServiceItemId { get; set; }
        public int PartId { get; set; }    
        public string PartName { get; set; } = string.Empty;
        public int Qty { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}

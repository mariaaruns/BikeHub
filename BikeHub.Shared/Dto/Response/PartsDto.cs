using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Response
{
    public class PartsDto
    {
        public int PartId { get; set; }
        public int CategoryId { get; set; }
        public string PartName { get; set; }
        public decimal Price { get; set; }
    }
}

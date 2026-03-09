using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Request
{
    public class UpdateBrandDto
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string? ImageUrl { get; set; }
        public byte[]? Imagebyte { get; set; }
    }
}

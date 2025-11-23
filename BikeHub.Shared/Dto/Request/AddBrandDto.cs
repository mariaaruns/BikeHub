using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Request
{
    public class AddBrandDto
    {
        public string   BrandName { get; set; }

        public string?  ImageUrl  { get; set; } 

        public IFormFile? BrandImage { get; set; }  
    }
}

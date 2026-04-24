using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Response.Auth
{
    public class IdentityErrorResult
    {
        public bool IsSuccess { get; set; }
        public string message { get; set; } = string.Empty;
        
    }
}

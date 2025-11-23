using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Response
{
    public class JwtResponse
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

       public string? ProfileImage { get; set; }
    }
}

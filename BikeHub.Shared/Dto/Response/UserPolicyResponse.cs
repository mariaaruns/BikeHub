using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Response
{
    public class UserPolicyResponse
    {

        public int PolicyId { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public bool? Haspermission { get; set; }

    }
}

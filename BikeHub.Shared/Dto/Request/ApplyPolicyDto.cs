using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Request
{
    public class ApplyPolicyDto
    {
        public int PolicyId { get; set; }
        public bool HasPermission { get; set; }
    }
}

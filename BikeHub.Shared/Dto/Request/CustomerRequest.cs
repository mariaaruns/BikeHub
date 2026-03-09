using BikeHub.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Request
{
    public class CustomerRequest:PaginationBaseModel
    {
        public string? CustomerName { get; set; }
    }
}

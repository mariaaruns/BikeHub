using BikeHub.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Request
{
    public class GetProductsDto:PaginationBaseModel
    {
        public string? ProductNameFilter { get; set; }
        //public int PageNumber { get; set; } = 1;
        //public int PageSize { get; set; } = 10;
    }
}

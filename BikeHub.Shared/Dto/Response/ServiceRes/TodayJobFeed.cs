using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Response.ServiceRes
{
    public  class TodayJobFeed
    {
           public long ServiceJobId { get; set; }
           public  string Mechanic { get; set; }
           public string JobId{ get; set; }
           public string?  Service { get; set; }
            public string? CustomerName { get; set; }
            public string? Status { get; set; }
            public DateTime CreatedAt{ get; set; }
    }
}

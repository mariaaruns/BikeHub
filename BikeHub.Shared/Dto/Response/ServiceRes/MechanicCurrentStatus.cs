using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Response.ServiceRes
{
    public class MechanicCurrentStatus
    {
        public long MechanicId { get; set; }

        public string Mechanic { get; set; }

        public int Pending { get; set; }

        public int Active { get; set; }
        public int Done { get; set; }

        public int Workload { get; set; }

        public string CurrentJob { get; set; }

    }
}

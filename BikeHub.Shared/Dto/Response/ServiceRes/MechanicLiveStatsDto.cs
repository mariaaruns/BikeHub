using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.Response.ServiceRes
{
    public class MechanicLiveStatsDto
    {
      
            public int TotalMechanics { get; set; }
            public int JobsToday { get; set; }
            public int BusyMechanics { get; set; }
            public int AvailableMechanics { get; set; }
            public int OverloadedMechanics { get; set; }
        
    }
}

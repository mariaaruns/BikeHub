using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.ReportsDto
{
    public class MechanicProductivityDto
    {
        public long MechanicId { get; set; }
        public string UserName { get; set; }
        public int JobsAssigned { get; set; }
        public int TotalMinutes { get; set; }
        public int AvgMinutesPerJob { get; set; }
        public int JobsCompleted { get; set; }
    }
}

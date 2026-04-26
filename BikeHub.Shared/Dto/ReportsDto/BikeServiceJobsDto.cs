using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Shared.Dto.ReportsDto
{
    public class BikeServiceJobsDto
    {
        public long ServiceJobId { get; set; }

        public string JobCardNumber { get; set; }

        public string CustomerId { get; set; }
        public string CustomerName { get; set; }

        public string ServiceStatus { get; set; }   

        public DateTime? CreatedDate { get; set; }

        public int ActualDuration { get; set; }
        public int JobTotal { get; set; }
        public int LineItems { get; set; }   



    }
}

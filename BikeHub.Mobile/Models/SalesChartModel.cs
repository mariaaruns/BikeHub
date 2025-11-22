using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Mobile.Models
{
    public class SalesChartModel
    {
        public string Month { get; set; }

        public double Target { get; set; }

        public SalesChartModel(string xValue, double yValue)
        {
            Month = xValue;
            Target = yValue;
        }
    }
}

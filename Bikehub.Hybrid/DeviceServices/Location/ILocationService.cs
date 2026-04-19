using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Sensor= Microsoft.Maui.Devices.Sensors;


namespace Bikehub.Hybrid.DeviceServices.Location
{
    public interface ILocationService
    {
        Task<bool> RequestLocationPermission();
        Task<Sensor.Location?> GetCurrentLocationAsync();
    }
}

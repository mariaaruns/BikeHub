using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikehub.Hybrid.DeviceServices.Location
{
    public class LocationService:ILocationService
    {

        public async Task<bool> RequestLocationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            return status == PermissionStatus.Granted;
        }

        public async Task<Microsoft.Maui.Devices.Sensors.Location?> GetCurrentLocationAsync()
        {
            try
            {
                var hasPermission = await RequestLocationPermission();

                if (DeviceInfo.Platform != DevicePlatform.WinUI && !hasPermission)
                    return null;

                var request = new GeolocationRequest(GeolocationAccuracy.High);
                var location = await Geolocation.Default.GetLocationAsync(request);

                if (location == null)
                {
                #if WINDOWS
                                    await Launcher.Default.OpenAsync("ms-settings:privacy-location");
                #endif
                }

                return location;
            }
            catch
            {
                #if WINDOWS
                                    await Launcher.Default.OpenAsync("ms-settings:privacy-location");
#endif
                return null;
            }
        }
        //public async Task<Microsoft.Maui.Devices.Sensors.Location?> GetCurrentLocationAsync()
        //{
        //    try
        //    {
            
        //                    var hasPermission = await RequestLocationPermission();
            
        //                    if (!hasPermission)
        //                        return null;
            
        //        var request = new GeolocationRequest(GeolocationAccuracy.High);
        //        return await Geolocation.Default.GetLocationAsync(request);
                
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

    }
}

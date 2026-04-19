using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using toolkit= CommunityToolkit.Maui.Alerts;


namespace Bikehub.Hybrid.DeviceServices.Toast
{
    public class ToastService : IToastService
    {
        public async Task Show(string message)
        {
            #if ANDROID||IOS
                var toast = toolkit.Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Short, 12);
                await toast.Show();
            #elif WINDOWS
                     var snackbar = CommunityToolkit.Maui.Alerts.Snackbar.Make(message);
                    await snackbar.Show();
            #endif

        }
    }
}

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Networking;
using System.Threading.Tasks;

namespace BikeHub.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Application.Current.UserAppTheme = AppTheme.Light;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private async void Connectivity_ConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {

                var toast = Toast.Make("No internet connection", ToastDuration.Long, 14);
                await toast.Show();
            }
            else { 
                    var toast = Toast.Make("Back Online", ToastDuration.Long, 14);
                    await toast.Show();
            }
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected  override async void OnStart()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {

                var toast = Toast.Make("No internet connection", ToastDuration.Long, 14);
               await toast.Show();
            }
           
        }

      
    }
}
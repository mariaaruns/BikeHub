using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Bikehub.Hybrid
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "Bikehub.Hybrid" };
        }

        private async void Connectivity_ConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            String message = e.NetworkAccess != NetworkAccess.Internet ? "No internet connection" : "Back Online";
            

                #if ANDROID || ios
                                var toast = Toast.Make(message, ToastDuration.Long, 14);
                                await toast.Show();
                #elif WINDOWS
                                var snackbar = CommunityToolkit.Maui.Alerts.Snackbar.Make(message);
                                await snackbar.Show();
                #endif
            

            }
    } 
}


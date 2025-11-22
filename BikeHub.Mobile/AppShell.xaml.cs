using BikeHub.Mobile.Pages;
using System.Threading.Tasks;

namespace BikeHub.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            this.Navigated += OnShellNavigated;
            
            Routing.RegisterRoute(nameof(AddEditProductPage), typeof(AddEditProductPage));
            Routing.RegisterRoute(nameof(AddEditCustomer), typeof(AddEditCustomer));
            Routing.RegisterRoute(nameof(AddEditCategory), typeof(AddEditCategory));
            Routing.RegisterRoute(nameof(AddEditOrders), typeof(AddEditOrders));
            Routing.RegisterRoute(nameof(AddEditUsers), typeof(AddEditUsers));
            Routing.RegisterRoute(nameof(AddEditBrand), typeof(AddEditBrand));
            Routing.RegisterRoute(nameof(OrderPlacedPage), typeof(OrderPlacedPage));
            Routing.RegisterRoute(nameof(OrderCheckoutPage), typeof(OrderCheckoutPage));
            Routing.RegisterRoute(nameof(CustomerDetailsPage), typeof(CustomerDetailsPage));


        }

        private void OnShellNavigated(object sender, ShellNavigatedEventArgs e)
        {
            // Get the current page
            var currentPage = (Shell.Current?.CurrentPage as ContentPage);
            if (currentPage != null)
            {
                PageTitleLabel.Text = currentPage.Title;
            }
        }


        private async void AvatarButton_Clicked(object sender, EventArgs e)
        {
            var username =await SecureStorage.GetAsync("user_email");
            if (string.IsNullOrEmpty(username))
                username = "Account";

            var action = await Shell.Current.DisplayActionSheet(username, "Cancel", null,FlowDirection.MatchParent, "Settings", "Logout");
            if (action == "Logout")
            {
                 SecureStorage.Remove("access_token");
                 SecureStorage.Remove("access_token_expires");

                await Shell.Current.GoToAsync("//LoginPage");
            }
            else if (action == "Settings")
            {
                // Navigate to the user/settings tab/page. UserPage route exists in the TabBar.
                await Shell.Current.GoToAsync("//UserPage");
            }
        }


    }
}

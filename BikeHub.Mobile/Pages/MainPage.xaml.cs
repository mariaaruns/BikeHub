using BikeHub.Mobile.ViewModel;

namespace BikeHub.Mobile.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly DashboardViewModel _vm;
        private bool _dashboardLoaded;

        public MainPage(DashboardViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext= _vm=viewModel;
        }
        private void pickerButton_Clicked(object sender, System.EventArgs e)
        {
            this.datepicker.IsOpen = true;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
          //  var cts = new CancellationTokenSource();
            if (_dashboardLoaded) return;
            _dashboardLoaded = true;

            if (_vm?.LoadallDataDashboardCommand is not null && _vm.LoadallDataDashboardCommand.CanExecute(null))
            {
                await _vm.LoadallDataDashboardCommand.ExecuteAsync(null);
            }

          
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // 3. If the user leaves the page while it's still loading, 
            // tell the ViewModel to stop.
            if (_vm?.LoadallDataDashboardCommand is not null && _vm.LoadallDataDashboardCommand.IsRunning)
            {
                _vm.LoadallDataDashboardCommand.Cancel();
            }
        }

    }
}

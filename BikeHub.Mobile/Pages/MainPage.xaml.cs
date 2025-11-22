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

            if (_dashboardLoaded) return;
            _dashboardLoaded = true;
            if (_vm?.LoadDashboardCommand is not null && _vm.LoadDashboardCommand.CanExecute(null))
            {
                await _vm.LoadDashboardCommand.ExecuteAsync(null);
            }

            if(_vm?.LoadSalesChartCommand is not null && _vm.LoadSalesChartCommand.CanExecute(null))
            {
                 await _vm.LoadSalesChartCommand.ExecuteAsync(null);
            }
        }
      
    }
}

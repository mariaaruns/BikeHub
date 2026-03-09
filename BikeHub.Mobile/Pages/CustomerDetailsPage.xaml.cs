using BikeHub.Mobile.ViewModel;

namespace BikeHub.Mobile.Pages;

public partial class CustomerDetailsPage : ContentPage
{
    private readonly CustomerDetailViewModel _vm;

    private bool isInitialized;

    public CustomerDetailsPage(CustomerDetailViewModel viewModel)
	{
		InitializeComponent();
        BindingContext=_vm= viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (isInitialized)
            return;

        if (_vm.LoadCustomerDetailCommand.CanExecute(null) && _vm.LoadCustomerDetailCommand is not null)
        {
            await _vm.LoadCustomerDetailCommand.ExecuteAsync(null);
            isInitialized = true;
        }

    }
}
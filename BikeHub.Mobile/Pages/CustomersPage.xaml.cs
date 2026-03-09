using BikeHub.Mobile.ViewModel;

namespace BikeHub.Mobile.Pages;

public partial class CustomersPage : ContentPage
{
	private readonly CustomerViewModel _vm;
    private bool isInitialized;
    public CustomersPage (CustomerViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext =_vm= viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (isInitialized) 
			return;	

        if (_vm.LoadCustomersCommand.CanExecute(null) && _vm.LoadCustomersCommand is not null)
		{
			await _vm.LoadCustomersCommand.ExecuteAsync(null);
            isInitialized = true;
        }
    }
}
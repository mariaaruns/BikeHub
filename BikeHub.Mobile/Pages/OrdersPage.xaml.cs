using Android.Support.Customtabs.Trusted;
using BikeHub.Mobile.ViewModel;

namespace BikeHub.Mobile.Pages;

public partial class OrdersPage : ContentPage
{
	private bool isInitialized;
    private readonly OrderViewModel _vm;
    public OrdersPage(OrderViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext=_vm=viewModel;
    }


	protected override  async void OnAppearing()
	{
		base.OnAppearing();
		
		if (isInitialized) 
			return;

		if (_vm.LoadOrderstausCommand is not null && _vm.LoadOrderstausCommand.CanExecute(null)) {

            await  _vm.LoadOrderstausCommand.ExecuteAsync(null);
			isInitialized = true;

        }
       
        if (_vm.LoadOrdersCommand is not null && _vm.LoadOrdersCommand.CanExecute(null))
		{
			await _vm.LoadOrdersCommand.ExecuteAsync(null);
		}

    }


}
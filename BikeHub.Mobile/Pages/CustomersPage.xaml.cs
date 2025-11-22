using BikeHub.Mobile.ViewModel;

namespace BikeHub.Mobile.Pages;

public partial class CustomersPage : ContentPage
{
	public CustomersPage (CustomerViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}
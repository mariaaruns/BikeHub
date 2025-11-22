using BikeHub.Mobile.ViewModel;

namespace BikeHub.Mobile.Pages;

public partial class AddEditCustomer : ContentPage
{
	public AddEditCustomer(CustomerViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
    }
}
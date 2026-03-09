using BikeHub.Mobile.ViewModel;

namespace BikeHub.Mobile.Pages;

public partial class AddEditCustomer : ContentPage
{
	private readonly AddEditCustomerViewmodel _vm;
	
	public AddEditCustomer(AddEditCustomerViewmodel viewModel)
	{
		InitializeComponent();
		this.BindingContext = _vm = viewModel;
    }

}
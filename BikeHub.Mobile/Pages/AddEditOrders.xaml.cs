using BikeHub.Mobile.ViewModel;

namespace BikeHub.Mobile.Pages;

public partial class AddEditOrders : ContentPage
{
	public AddEditOrders(OrderViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext=viewModel;
    }
}
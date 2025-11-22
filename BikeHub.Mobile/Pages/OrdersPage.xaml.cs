using BikeHub.Mobile.ViewModel;

namespace BikeHub.Mobile.Pages;

public partial class OrdersPage : ContentPage
{
	public OrdersPage(OrderViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext=viewModel;
    }

  
}
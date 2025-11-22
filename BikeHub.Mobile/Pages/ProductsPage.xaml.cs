using BikeHub.Mobile.Controls;
using BikeHub.Mobile.ViewModel;
using BikeHub.Shared.Dto.Response;
using CommunityToolkit.Maui.Views;

namespace BikeHub.Mobile.Pages;

public partial class ProductsPage : ContentPage
{
	public ProductsPage(ProductsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
   


}
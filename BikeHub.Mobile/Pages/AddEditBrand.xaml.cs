using BikeHub.Mobile.ViewModel;

namespace BikeHub.Mobile.Pages;

public partial class AddEditBrand : ContentPage
{
	public AddEditBrand(ProductsViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}
using BikeHub.Mobile.ViewModel;

namespace BikeHub.Mobile.Pages;

public partial class AddEditCategory : ContentPage
{
	public AddEditCategory(ProductsViewModel viewModel)
	{
		InitializeComponent();

		this.BindingContext = viewModel;
	}
}
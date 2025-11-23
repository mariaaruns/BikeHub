using BikeHub.Mobile.ViewModel;

namespace BikeHub.Mobile.Pages;

public partial class AddEditBrand : ContentPage
{
	public AddEditBrand(AddEditBrandViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
	}
}
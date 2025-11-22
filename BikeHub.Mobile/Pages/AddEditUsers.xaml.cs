using BikeHub.Mobile.ViewModel;

namespace BikeHub.Mobile.Pages;

public partial class AddEditUsers : ContentPage
{
	public AddEditUsers(UserViewModel viewModel)
	{
		InitializeComponent();
		this.BindingContext = viewModel;
    }
}
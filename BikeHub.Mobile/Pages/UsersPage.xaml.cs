using BikeHub.Mobile.ViewModel;

namespace BikeHub.Mobile.Pages;

public partial class UsersPage : ContentPage
{
	public UsersPage(UserViewModel viewModel)
	{
		InitializeComponent();

		this.BindingContext = viewModel;
	}
}
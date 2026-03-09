using BikeHub.Mobile.ViewModel;

namespace BikeHub.Mobile.Pages;

public partial class UsersPage : ContentPage
{
    private bool isInitialized = false;
    private readonly UserViewModel _vm;
    public UsersPage(UserViewModel viewModel)
	{
		InitializeComponent();

		this.BindingContext = _vm=viewModel;
	}


    protected override void OnAppearing()
    {
       
        base.OnAppearing();

        if (isInitialized) return;

        if (_vm.LoadUsersCommand is not null && _vm.LoadUsersCommand.CanExecute(null))
        {
            _vm.LoadUsersCommand.Execute(null);
            isInitialized = true;
        }

    }
}
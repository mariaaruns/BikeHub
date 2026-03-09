using BikeHub.Mobile.Helper;

namespace BikeHub.Mobile.Pages;

public partial class LoadingPage : ContentPage
{
	public LoadingPage()
	{
		InitializeComponent();
        //Loaded += LoadingPage_Loaded;
	}


    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(100);

        bool loggedIn = await IsLoggedInAsync();

        if (loggedIn)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
    private async Task<bool> IsLoggedInAsync()
    {
        var token = await SecureStorage.GetAsync("access_token");
        //var expiry = await SecureStorage.GetAsync("access_token_expires");

        //if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(expiry))
        //    return false;

        //if (!DateTime.TryParse(expiry, out var expiryDate))
        //    return false;

        return JwtHelper.IsTokenExpired(token) == false;
    }
}
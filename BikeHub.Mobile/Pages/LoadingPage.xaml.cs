namespace BikeHub.Mobile.Pages;

public partial class LoadingPage : ContentPage
{
	public LoadingPage()
	{
		InitializeComponent();
        Loaded += LoadingPage_Loaded;
	}

    private void LoadingPage_Loaded1(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private async void LoadingPage_Loaded(object sender, EventArgs e)
    {
        await Task.Delay(100); // allow UI to load

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
        var expiry = await SecureStorage.GetAsync("access_token_expires");

        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(expiry))
            return false;

        if (!DateTime.TryParse(expiry, out var expiryDate))
            return false;

        return expiryDate > DateTime.UtcNow;
    }
}
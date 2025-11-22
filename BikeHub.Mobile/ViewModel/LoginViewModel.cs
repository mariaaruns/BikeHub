using BikeHub.Mobile.ApiServices;
using BikeHub.Mobile.Pages;
using BikeHub.Shared.Dto.Request;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BikeHub.Mobile.ViewModel
{
    public partial class LoginViewModel :ObservableObject
    {

        private readonly IUserApi _userApi;
        public LoginViewModel(IUserApi userApi)
        {
            _userApi = userApi;
        }

        [ObservableProperty] //UserName
        private string _userName;

        [ObservableProperty] //UserName
        private string _password;

        [ObservableProperty]
        private bool _isBusy;

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;

                if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
                {
                    await Application.Current.MainPage.DisplayAlert("Validation", "Email and password are required.", "OK");
                    return;
                }


                var dto = new LoginDto
                {
                    Email = this.UserName,
                    Password = this.Password,
                    RememberMe = true
                };

                var result = await _userApi.Login(dto);
                if (result is null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No response from server.", "OK");
                    return;
                }


                if (result.Status && result.Data is not null && !string.IsNullOrWhiteSpace(result.Data.Token))
                {
                    // Save token securely
                    await SecureStorage.SetAsync("access_token", result.Data.Token);

                    // Optionally save other info
                    await SecureStorage.SetAsync("access_token_expires", result.Data.Expires.ToString("o"));
                    if (!string.IsNullOrWhiteSpace(result.Data.Email))
                        await SecureStorage.SetAsync("user_email", result.Data.Email);

                    // Navigate to main page (reset navigation stack)
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                    return;
                }

                var message = result.Message ?? "Login failed";
                string errorsText = string.Empty;

                if (result.Errors is string s)
                {
                    errorsText = s;
                }
                else if (result.Errors is string[] arr)
                {
                    errorsText = string.Join(Environment.NewLine, arr);
                }
                else if (result.Errors != null)
                {
                    errorsText = result.Errors.ToString();
                }

                var alertText = string.IsNullOrWhiteSpace(errorsText) ? message : $"{message}\n{errorsText}";
                await Application.Current.MainPage.DisplayAlert("Login failed", alertText, "OK");
                //    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");


            }
            catch (ApiException apiEx)
            {
                // Refit ApiException (if thrown) - show server error or validation details
                var err = await apiEx.GetContentAsAsync<ApiResponse<object>>();
                var details = err?.Error?.ToString() ?? apiEx.Message;
                await Application.Current.MainPage.DisplayAlert("Server error", details, "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

      
    }
}

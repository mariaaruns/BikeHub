using Bikehub.Hybrid.Authhandler;
using Bikehub.Hybrid.Services.Http.Auth;
using BikeHub.Shared.Dto.Request;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikehub.Hybrid.Components.Pages
{
    public partial class Login : ComponentBase
    {
        [Inject]
        private IAuthService AuthService { get; set; } = default!;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private CustomAuthStateProvider AuthProvider { get; set; } = default!;


        private LoginDto _loginModel = new();
        private string _errorMessage = string.Empty;
        private bool _isLoading = false;
        private bool _showPassword = false;

        [CascadingParameter]
        private Task<AuthenticationState> AuthState { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthState;
            if (auth.User.Identity?.IsAuthenticated == true)
                Navigation.NavigateTo("/", replace: true);
        }
        private void TogglePassword() => _showPassword = !_showPassword;

        private async Task HandleLogin()
        {
            _isLoading = true;
            _errorMessage = string.Empty;
            try
            {
                var result = await AuthService.Login(_loginModel);

                if (result.Status && !string.IsNullOrEmpty(result.Data.Token))
                {
                    await SecureStorage.SetAsync("auth_token", result.Data.Token);
                    AuthProvider.NotifyAuthStateChanged();
                    Navigation.NavigateTo("/", replace: true);
                }
                else
                {
                    _errorMessage = result.Message ?? "Login failed. Please try again.";
                }
            }
            catch (HttpRequestException)
            {
                _errorMessage = "Unable to reach the server. Check your connection.";
            }
            catch (Exception ex)
            {
                _errorMessage = "An unexpected error occurred.";
            }
            finally
            {
                _isLoading = false;
            }
        }


    }
}

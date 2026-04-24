using Microsoft.AspNetCore.Components;

namespace Bikehub.Hybrid.Authhandler
{
    public class AuthorizedHandler : DelegatingHandler
    {
        private readonly NavigationManager _navigation;
        private readonly CustomAuthStateProvider _authProvider;

        public AuthorizedHandler(NavigationManager navigation, CustomAuthStateProvider authProvider)
        {
            _navigation = navigation;
            _authProvider = authProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            // Handle 401 Unauthorized
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                SecureStorage.Remove("auth_token");
                _authProvider.NotifyLogout();
                
            }

            return response;
        }
    }
}
using BikeHub.Shared.Common;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.JsonWebTokens; // ✅ correct namespace for MAUI
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Bikehub.Hybrid.Authhandler
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly UserSession _session;
        private readonly IHttpClientFactory _httpFactory;
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

        public CustomAuthStateProvider(UserSession session, IHttpClientFactory http)
        {
            _session = session;
            _httpFactory = http;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await SecureStorage.GetAsync("auth_token");

            if (string.IsNullOrEmpty(token))
                return new AuthenticationState(_anonymous);

            if (IsTokenExpired(token))
            {
                SecureStorage.Remove("auth_token");

                return new AuthenticationState(_anonymous);
            }

            if (string.IsNullOrEmpty(_session.Token))
            {
                ParseTokenAndPopulateSession(token);

                _session.Policies.Clear();
                //  _session.Policies = null;

            }

            if (_session.Policies == null || !_session.Policies.Any() ) 
            { 
            await FetchPoliciesFromApi(token);
            }
            
            

            return CreateAuthState();
        }

        // ✅ Call from your login page after storing the token
        public void NotifyAuthStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        // ✅ Call from your logout page
        public void NotifyLogout()
        {
            _session.UserId = string.Empty;
            _session.Username = string.Empty;
            _session.Token = string.Empty;
            _session.Roles = new();
            _session.Policies = new();
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(_anonymous)));
        }

        private bool IsTokenExpired(string token)
        {
            try
            {
                
                var handler = new JsonWebTokenHandler();
                var jwt = handler.ReadJsonWebToken(token);
                return jwt.ValidTo < DateTime.UtcNow.AddSeconds(-30);
            }
            catch
            {
                return true; //treat malformed tokens as expired
            }
        }

        private void ParseTokenAndPopulateSession(string token)
        {
            var handler = new JsonWebTokenHandler();
            var jwt = handler.ReadJsonWebToken(token);

            _session.Token = token;
            // ✅ JsonWebToken uses GetClaims() differently — use Claims property
            _session.Roles = jwt.Claims
                .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                .Select(c => c.Value)
                .ToHashSet();

            _session.Username = jwt.Claims.FirstOrDefault(c => c.Type == "name"
                                                            || c.Type == "unique_name"
                                                            || c.Type == "email")?.Value ?? "User";
            _session.UserId = jwt.Claims.Where(c => c.Type == "sub").FirstOrDefault()?.Value;


        }

        private async Task FetchPoliciesFromApi(string token)
        {
            try
            {
                var _http= _httpFactory.CreateClient("BikeHub");
                    var response = await _http.GetFromJsonAsync<ApiResponse<List<string>>>("/api/userPolicyCached");

                if (response.Data != null)
                    _session.Policies = response.Data.ToHashSet();
                else
                    _session.Policies = new();
            }
            catch
            {
                _session.Policies = new();
            }
        }

        private AuthenticationState CreateAuthState()
        {
            var claims = new List<Claim>();
            foreach(var role in _session.Roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            if (_session.Policies != null)
                foreach (var policy in _session.Policies)
                claims.Add(new Claim("Permission",policy));


            //_session.Roles.ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
            //if (_session.Policies != null)
            //    _session.Policies.ForEach(p => claims.Add(new Claim("Permission", p)));

            var identity = new ClaimsIdentity(claims, "JwtAuth");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
    }
}
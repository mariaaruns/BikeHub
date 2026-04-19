using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Bikehub.Hybrid.Services.Http.Auth
{
    public interface IAuthService
    {
        Task<ApiResponse<JwtResponse>> Login(LoginDto dto);

    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BikeHub");
        }
        public async Task<ApiResponse<JwtResponse>> Login(LoginDto dto)
        {

            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/user/login", dto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<JwtResponse>>();
                }

                return new ApiResponse<JwtResponse>
                {
                    Status = false,
                    Message = $"Server returned {response.StatusCode}"
                };
            }
            
            catch (Exception ex)
            {
                // This catches the "No JSON tokens" error or Network errors
                return new ApiResponse<JwtResponse>
                {
                    Status= false,
                    Message = "Could not reach API or received invalid response."
                };
            
        }
        }


    }
}

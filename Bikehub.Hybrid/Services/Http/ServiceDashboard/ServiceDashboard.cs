using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Response.ServiceRes;
using System.Net.Http.Json;

namespace Bikehub.Hybrid.Services.Http.ServiceDashboard
{
    public class ServiceDashboard : IServiceDashboard
    {
        private readonly HttpClient _httpClient;
        public ServiceDashboard(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BikeHub");
        }

        public async Task<ApiResponse<IEnumerable<AssignedJobDto>>> AssignedJobsAsync(int mechanicId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/services/mechanic/assigned-jobs/{mechanicId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<AssignedJobDto>>>();
                }

                return new ApiResponse<IEnumerable<AssignedJobDto>>
                {
                    Status = false,
                    Message = $"Server returned {response.StatusCode}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<AssignedJobDto>>
                {
                    Status = false,
                    Message = "Could not reach API or received invalid response."
                };
            }
        }

        public async Task<ApiResponse<string>> CompleteJobAsync(long jobId)
        {
            try 
            { 
                var response = await _httpClient.PatchAsync($"/api/services/complete-job/{jobId}", null);

                return new ApiResponse<string>
                {
                    Status = response.IsSuccessStatusCode,
                    Message = response.IsSuccessStatusCode ? "Job started successfully." : $"Failed to start job. Server returned {response.StatusCode}"
                };

            }
            catch (Exception)
            {
                return new ApiResponse<string>
                {
                    Status = false,
                    Message = "Could not reach API or received invalid response."
                };
            }
        }

        public async Task<ApiResponse<ServiceJobDetailDto>> JobDetailsAsync(long jobId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/services/job/{jobId}");

                if (response.IsSuccessStatusCode)
                {

                    return await response.Content.ReadFromJsonAsync<ApiResponse<ServiceJobDetailDto>>();
                }

                return new ApiResponse<ServiceJobDetailDto>()
                {
                    Status = false,
                    Message = $"Server returned {response.StatusCode}"
                };

            }
            catch (Exception)
            {
                return new ApiResponse<ServiceJobDetailDto>()
                {
                    Status = false,
                    Message = "Could not reach API or received invalid response."
                };
            }
        }

        public async Task<ApiResponse<MechanicTaskSummayDto>> MechanicWorkSummaryAsync(int mechanicId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/services/mechanic/summary/{mechanicId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<MechanicTaskSummayDto>>();
                }

                return new ApiResponse<MechanicTaskSummayDto>
                {
                    Status = false,
                    Message = $"Server returned {response.StatusCode}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<MechanicTaskSummayDto>
                {
                    Status = false,
                    Message = "Could not reach API or received invalid response."
                };
            }
        }

        public async Task<ApiResponse<string>> StartJobAsync(long jobId)
        {
            try
            {
                var response = await _httpClient.PatchAsync($"/api/services/start-job/{jobId}", null);

                return new ApiResponse<string>
                {
                    Status = response.IsSuccessStatusCode,
                    Message = response.IsSuccessStatusCode ? "Job started successfully." : $"Failed to start job. Server returned {response.StatusCode}"
                };
            }
            catch (Exception)
            {
                         return new ApiResponse<string>
                            {
                                Status = false,
                                Message = "Could not reach API or received invalid response."
                            };
            }


        }
    }
}

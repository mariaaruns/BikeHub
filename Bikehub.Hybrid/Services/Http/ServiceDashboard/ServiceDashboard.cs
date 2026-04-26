using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
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

        public async Task<ApiResponse<bool>> AddServiceItems(AddServiceItemsDto req)
        {
            try
            {
                var request= new HttpRequestMessage(HttpMethod.Post, "/api/services/items-add")
                {
                    Content = JsonContent.Create(req)
                };

                var response = await _httpClient.SendAsync(request);
         
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
                }
              
                return new ApiResponse<bool>
                {
                    Status = false,
                    Message = $"Server returned {response.StatusCode}"
                };
            }
            catch (Exception)
            {
                return new ApiResponse<bool>
                {
                    Status = false,
                    Message = "Could not reach API or received invalid response."
                };
            }
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

        public async Task<ApiResponse<DropdownDto[]>> DropdownLookup(string value)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/Dropdown?type={value}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<DropdownDto[]>>();
                }
                return new ApiResponse<DropdownDto[]>()
                {
                    Status = false,
                    Message = $"Server returned {response.StatusCode}"
                };
            }
            catch (Exception)
            {
                return new ApiResponse<DropdownDto[]>()
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

        public async Task<ApiResponse<IEnumerable<PartsDto>>> PartsList()
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/services/parts");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<PartsDto>>>();
                }

                return new ApiResponse<IEnumerable<PartsDto>>
                {
                    Status = false,
                    Message = $"Server returned {response.StatusCode}"
                };
            }
            catch (Exception)
            {
                return new ApiResponse<IEnumerable<PartsDto>>
                {
                    Status = false,
                    Message = "Could not reach API or received invalid response."
                };
            }
        }

        public async Task<ApiResponse<List<ServiceItemDto>>> ServiceItems(long jobId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/services/items/{jobId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ApiResponse<List<ServiceItemDto>>>();
                }

                return new ApiResponse<List<ServiceItemDto>>
                {
                    Status = false,
                    Message = $"Server returned {response.StatusCode}"
                };
            }
            catch (Exception)
            {
                return new ApiResponse<List<ServiceItemDto>>
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

using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using BikeHub.Shared.Dto.Response.ServiceRes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Bikehub.Hybrid.Services.Http.ServiceDashboard
{
    public interface IServiceDashboard
    {
        Task<ApiResponse<MechanicTaskSummayDto>> MechanicWorkSummaryAsync(int mechanicId);

        Task<ApiResponse<IEnumerable<AssignedJobDto>>> AssignedJobsAsync(int mechanicId);

        Task<ApiResponse<ServiceJobDetailDto>> JobDetailsAsync(long jobId);

        Task<ApiResponse<string>> StartJobAsync(long jobId);

        Task<ApiResponse<string>> CompleteJobAsync(long jobId);

        Task<ApiResponse<List<ServiceItemDto>>> ServiceItems(long jobId);
        Task<ApiResponse<bool>> AddServiceItems(AddServiceItemsDto req);

        Task<ApiResponse<DropdownDto[]>> DropdownLookup(string value);


        Task<ApiResponse<IEnumerable<PartsDto>>> PartsList();
    }

}

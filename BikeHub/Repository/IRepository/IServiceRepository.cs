using BikeHub.Shared.Dto.Response;
using BikeHub.Shared.Dto.Response.ServiceRes;

namespace BikeHub.Repository.IRepository
{
    public interface IServiceRepository
    {

        Task<MechanicLiveStatsDto> GetLiveMechanicStatsAsync();
        Task<IEnumerable<MechanicCurrentStatus>> GetMechanicStatusAsync();
        Task<IEnumerable<TodayJobFeed>> GetDailyJobsByStatusAsync(DateTime? date,int? serviceStatus); 
        Task<IEnumerable<AssignedJobDto>> GetMechanicAssignedJobsAsync(int mechanicId);
        Task<MechanicTaskSummayDto> GetMechanicWorkSummaryAsync(int mechanicId);
        Task StartJobAsync(int jobId);
        Task CompleteJobAsync(int jobId);
        Task UpdateJobStatusAsync(int status,int jobId);
        Task<ServiceJobDetailDto> GetJobByIdAsync(int jobId);
        Task<ServiceItemDto> GetServiceItemsAsync(int jobId);
        Task AssignNewJobAsync(CreateJobAssignmentDto dto);
        Task<IEnumerable<DropdownDto>> GetServiceStatusDropdownAsync();
    }
}

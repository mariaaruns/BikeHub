using BikeHub.Shared.Dto.Response;

namespace BikeHub.Repository.IRepository
{
    public interface IDashboardRepository
    {

        Task<DashboardResponseDto> GetCounts(int year,int month);
        Task<IEnumerable<SalesAmountByYearDto>> GetSalesAmountByYearAsync(int year);
    }
}

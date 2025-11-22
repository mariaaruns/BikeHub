using BikeHub.Shared.Dto.Response;

namespace BikeHub.Repository.IRepository
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<SalesAmountByYearDto>> GetSalesAmountByYearAsync(int year);
    }
}

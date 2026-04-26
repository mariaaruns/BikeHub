using BikeHub.Shared.Dto.ReportsDto;

namespace BikeHub.Repository.IRepository
{
    public interface IReportsRepository
    {

        Task<IEnumerable<CustomerOrderRevenueDto>> CustomerOrderRevenue(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<TopProductsByRevenueDto>> TopProductsByRevenue(DateTime fromDate, DateTime toDate);

        Task<IEnumerable<InventoryDto>> Inventory();

        Task<IEnumerable<BikeServiceJobsDto>> BikeServiceJobs(DateTime fromDate ,DateTime toDate);

        Task<IEnumerable<MechanicProductivityDto>> MechanicProductivity(DateTime Date);
    }
}

using BikeHub.Shared.Dto.ReportsDto;

namespace BikeHub.Service.Interface
{
    public interface IReportService
    {
        Task<(bool isSuccess,string Msg,string filePath)> CustomerOrderRevenue(DateTime fromDate, DateTime toDate);
        Task<(bool isSuccess, string Msg, string filePath)> TopProductsByRevenue(DateTime fromDate, DateTime toDate);

        Task<(bool isSuccess, string Msg, string filePath)> Inventory();

        Task<(bool isSuccess, string Msg, string filePath)> BikeServiceJobs(DateTime fromDate, DateTime toDate);

        Task<(bool isSuccess, string Msg, string filePath)> MechanicProductivity(DateTime Date);
    }
}

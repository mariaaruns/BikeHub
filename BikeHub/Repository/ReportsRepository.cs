using BikeHub.Repository.IRepository;
using BikeHub.Shared.Dto.ReportsDto;
using BikeHub.Shared.Dto.Response;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BikeHub.Repository
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly IDbConnection _connection;

        public ReportsRepository(IDbConnection connection)
        {
            this._connection = connection;
        }

        public async Task<IEnumerable<BikeServiceJobsDto>> BikeServiceJobs(DateTime fromDate, DateTime toDate)
        {
            IEnumerable<BikeServiceJobsDto> result = Enumerable.Empty<BikeServiceJobsDto>();

            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    result = await connection.QueryAsync<BikeServiceJobsDto>(
                    "sp001_Reports",
                    new { @key = "RecentBikeServiceJobs",@fromDate= fromDate, @toDate = toDate 
                    }, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public async Task<IEnumerable<CustomerOrderRevenueDto>> CustomerOrderRevenue(DateTime fromDate, DateTime toDate)
        {
            IEnumerable<CustomerOrderRevenueDto> result= Enumerable.Empty<CustomerOrderRevenueDto>();

            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    result = await connection.QueryAsync<CustomerOrderRevenueDto>(
                    "sp001_Reports",
                    new { @key = "CustomerOrderRevenueReport", @fromDate = fromDate, @toDate = toDate }
                    , commandType : CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public async Task<IEnumerable<InventoryDto>> Inventory()
        {
            IEnumerable<InventoryDto> result = Enumerable.Empty<InventoryDto>();

            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    result = await connection.QueryAsync<InventoryDto>(
                    "sp001_Reports",
                    new { @key = "InventoryReport", },commandType : CommandType.StoredProcedure );
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public async Task<IEnumerable<MechanicProductivityDto>> MechanicProductivity(DateTime Date)
        {

            IEnumerable<MechanicProductivityDto> result = Enumerable.Empty<MechanicProductivityDto>();

            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    result = await connection.QueryAsync<MechanicProductivityDto>(
                    "sp001_Reports",
                    new
                    {
                        @key = "MechanicProductivityLast30Days",
                        @fromDate = Date,
                    }, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public async Task<IEnumerable<TopProductsByRevenueDto>> TopProductsByRevenue(DateTime fromDate, DateTime toDate)
        {

            IEnumerable<TopProductsByRevenueDto> result = Enumerable.Empty<TopProductsByRevenueDto>();

            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    result = await connection.QueryAsync<TopProductsByRevenueDto>(
                    "sp001_Reports",
                    new
                    {
                        @key = "TopProductsByRevenue",
                        @fromDate = fromDate,
                        @toDate = toDate
                    }, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}

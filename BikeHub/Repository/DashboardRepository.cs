using BikeHub.DapperQuery;
using BikeHub.Repository.IRepository;
using BikeHub.Shared.Dto.Response;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace BikeHub.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IDbConnection _dbConnection;

        public DashboardRepository(IDbConnection dbConnection)
        {
            this._dbConnection = dbConnection;
        }
        public async Task<IEnumerable<SalesAmountByYearDto>> GetSalesAmountByYearAsync(int year)
        {
            try
            {
                var sql = DashboardQuery.DashBoardSalesChart;
                var parameters = new { Year = year };
                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    var result = await connection.QueryAsync<SalesAmountByYearDto>(sql, parameters);
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
            
            

        }
    }
}

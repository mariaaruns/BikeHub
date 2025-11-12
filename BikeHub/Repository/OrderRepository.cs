using BikeHub.DapperQuery;
using BikeHub.Repository.IRepository;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BikeHub.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnection _dbConnection;

        public OrderRepository(IDbConnection dbConnection)
        {
            this._dbConnection = dbConnection;
        }
        public async Task<PagedResult<OrdersDto>> GetOrdersAsync(GetOrderDto dto)
        {
            try
            {

                var parameters = new
                {
                    OrderId = dto.OrderId,
                    Offset = (dto.PageNumber - 1) * dto.PageSize,
                    PageSize = dto.PageSize,
                    OrderStatus = dto.OrderStatus,
                    OrderDate = dto.StartDate?.Date
                };

                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    var OrderList = await connection.QueryAsync<OrdersDto>(
                    OrderQuery.GetOrderList,
                    parameters,
                    commandType: CommandType.Text);

                    var totalCount = await connection.ExecuteScalarAsync<int>(
                        OrderQuery.GetOrderListCount, parameters,
                        commandType: CommandType.Text);

                    var PagedResult = new PagedResult<OrdersDto>
                    (
                        TotalRecords : totalCount,
                        Page : dto.PageNumber,
                        PageSize : dto.PageSize,
                        Data : OrderList.AsList()
                    );

                    return PagedResult;
                }
               
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}

using BikeHub.DapperQuery;
using BikeHub.Repository.IRepository;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Data;
using System.Transactions;

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
                        TotalRecords: totalCount,
                        Page: dto.PageNumber,
                        PageSize: dto.PageSize,
                        Data: OrderList.AsList()
                    );

                    return PagedResult;
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> AddOrder(AddOrderRequest req)
        {
            bool isSuccess = false;
            try
            {
                var AddOrderSql =OrderQuery.AddOrder;
                var OrderItemsSql = OrderQuery.AddOrderItems;

                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    var transaction = connection.BeginTransaction();
                    try
                    {
                        var lastInsertedOrderId = await connection.QuerySingleAsync<int>(AddOrderSql, new
                        {
                            req.CustomerId,
                            req.OrderStatus,
                            req.OrderDate,
                            req.RequiredDate,
                            req.ShippedDate,
                            req.StaffId
                        }, commandType: CommandType.Text, transaction: transaction);

                        int serialNo = 0;
                        foreach (var item in req.OrderItemRequests)
                        {
                            serialNo++;
                            await connection.ExecuteAsync(OrderItemsSql, new
                            {
                                ItemId = serialNo,
                                OrderId = lastInsertedOrderId,
                                item.ProductId,
                                item.Quantity,
                                item.UnitPrice,
                                item.Discount
                            }, commandType: CommandType.Text, transaction: transaction);
                        }

                        isSuccess = true;
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                        await transaction.RollbackAsync();

                    }

                }
            }
            catch (Exception)
            {
                isSuccess = false;
            }

            return isSuccess;

        }

        public async Task<bool> UpdateOrderStatus(string staus)
        {
            var sql = OrderQuery.UpdateOrderStatus;
            int isRowAffected = 0;
            
            try
            {
                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                { 
                  isRowAffected=  await connection.ExecuteAsync(sql, new { staus }, commandType: CommandType.Text);   
                }
            }
            catch (Exception)
            {

                throw;
            }

            return isRowAffected > 0;

        }



        
    }
}

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
                    OrderDate = dto.StartDate
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

        public async Task<bool> AddOrderAsync(AddOrderRequest req)
        {
            bool isSuccess = false;
            try
            {
                var AddOrderSql = OrderQuery.AddOrder;
                var OrderItemsSql = OrderQuery.AddOrderItems;
                var customer = CustomerQuery.GetCustomerById;
                var EmailTemplateSql = EmailTemplateQuery.EmailTemplate;
                var outboxSql = EmailTemplateQuery.InsertOutBoxMsg;

                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    await connection.OpenAsync();
                    var transaction = await connection.BeginTransactionAsync();
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
                                @ListPrice=item.UnitPrice,
                                item.Discount,
                                
                                
                            }, commandType: CommandType.Text, transaction: transaction);
                        }

                        var customerDetail = await connection.QuerySingleAsync<CustomersDto>
                            (customer, new { @Id = req.CustomerId }, transaction: transaction);

                        if (!string.IsNullOrEmpty(customerDetail.Email))
                        {
                            var emailTemplate = await connection.QueryFirstOrDefaultAsync<(string Subject, string HtmlBody)>
                                                    (EmailTemplateSql, new { @slugName = "Order-Ready-Delivery" }, transaction);


                            var htmlBody = emailTemplate.HtmlBody.Replace("{OrderNumber}", lastInsertedOrderId.ToString())
                                                                            .Replace("{Carrier}", "N/A")
                                                                            .Replace("{ItemCount}", "N/A");

                            //Insert into outbox for email notification

                            await connection.ExecuteAsync(outboxSql, new
                            {
                                @eventType = "Order-Ready-Delivery",
                                @payLoad = System.Text.Json.JsonSerializer.Serialize(new OutBoxMessagePayload
                                {
                                        Email = customerDetail.Email,
                                        CustomerName = customerDetail.CustomerName,
                                        Subject = emailTemplate.Subject,
                                        TemplateContent = htmlBody
                                })
                            }, transaction);
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

        public async Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto dto)
        {
            var sql = OrderQuery.UpdateOrderStatus;
            int isRowAffected = 0;

            try
            {
                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    isRowAffected = await connection.ExecuteAsync(sql, new { @OrderStatus=dto.OrderStatusId, @OrderId=dto.OrderId }, commandType: CommandType.Text);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return isRowAffected > 0;

        }

        public async Task<IEnumerable<DropdownDto>> GetOrderStatusDropdownAsync()
        {
            var sql = OrderQuery.GetOrderStatusLookup;
            try
            {
                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    var result = await connection.QueryAsync<DropdownDto>(sql);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<OrderDetailsDto> GetOrderDetailsAsync(int OrderId)
        {
            var sql = OrderQuery.GetOrderDetailByOrderId;
            try
            {

                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    var result = await connection.QueryAsync<OrderItemFlatDto>(sql, new { @OrderId=OrderId });

                    var GroupedResult = result
                            .GroupBy(o => new { o.OrderId,
                                o.StaffId,
                                o.StoreId,
                                o.RequiredDate,
                                o.ShippedDate,
                                o.OrderDate,
                                o.OrderStatus,
                                o.Image,
                                o.CustomerId,
                                o.CustomerName,
                                o.Email,
                                o.Phone })
                            .Select(g => new OrderDetailsDto
                             {
                                 OrderId=g.Key.OrderId,
                                 OrderStatus=g.Key.OrderStatus,
                                 CustomerId=g.Key.CustomerId,
                                 OrderDate=g.Key.OrderDate,
                                 RequiredDate=g.Key.RequiredDate,
                                 ShippedDate=g.Key.ShippedDate,
                                 StoreId=g.Key.StoreId,
                                 StaffId=g.Key.StaffId,
                                 CustomerName=g.Key.CustomerName,
                                 Email=g.Key.Email,
                                 Phone=g.Key.Phone,
                                 Image=g.Key.Image,
                                 OrderItems=g.Select(oi=>new OrderItemDetail
                                 {
                                     ItemId=oi.ItemId,
                                     ProductId=oi.ProductId,
                                     Quantity=oi.Quantity,
                                     ListPrice=oi.ListPrice,
                                     ProductName=oi.ProductName
                                 }).ToArray()

                             }).FirstOrDefault();

                    return GroupedResult;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

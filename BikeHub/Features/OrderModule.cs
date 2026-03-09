using BikeHub.Repository.IRepository;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BikeHub.Features
{
    public class OrderModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
      
            app.MapPost("/orders", async ([FromBody] GetOrderDto dto, IOrderRepository orderRepository) =>
            {
                try
                {
                    var result = await orderRepository.GetOrdersAsync(dto);

                    if (result.Data.Any()) {
                        
                        foreach (var item in result.Data)
                        {
                            item.Image = Path.Combine(commonInfo.BaseUrl, commonInfo.CUSTOMER_IMG_PATH, item.Image);
                        }
                        return Results.Ok(ApiResponse<PagedResult<OrdersDto>>.Success(result));
                    }
                        
                    return Results.Ok(ApiResponse<PagedResult<OrdersDto>>.Fail("No orders found"));

                }
                catch (Exception ex)
                {
                    
                    return Results.InternalServerError(ApiResponse<PagedResult<OrdersDto>>.Fail("Internal Server error, "+ex.Message));
                }
                

            })
               .WithTags("Orders")
               .WithName("GetOrders")
               .RequireAuthorization("ORDER_VIEW");


            app.MapPost("/addOrders", async ([FromBody]AddOrderRequest req ,IOrderRepository orderRepository) =>
            {
                try
                {
                    var IsValid = ModelValidator.Validate(req);

                    if (!IsValid.IsValid)
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail("Invalid Request", IsValid.Errors));
                    }

                    
                    var result= await orderRepository.AddOrderAsync(req);

                    if (result)
                        return Results.Ok(ApiResponse<string>.Success("Order added successfully"));

                    else
                        return Results.Ok(ApiResponse<string>.Fail("Failed to add order"));
                }
                catch (Exception)
                {

                    throw;
                }
               

            })
               .WithTags("Orders")
               .RequireAuthorization("ORDER_ADD");

            app.MapPut("/updateOrderStatus", async ([FromBody]UpdateOrderStatusDto req, [FromServices] IOrderRepository orderRepository) =>
            {
                try
                {
                    var IsValid = ModelValidator.Validate(req);

                    if (!IsValid.IsValid)
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail("Invalid Request", IsValid.Errors));
                    }

                    var result = await orderRepository.UpdateOrderStatusAsync(req);
                    if (!result)
                        return Results.Ok(ApiResponse<string>.Fail("Failed to update order status"));

                    return Results.Ok(ApiResponse<string>.Success("Order status updated successfully"));
                }
                catch (Exception)
                {

                    throw;
                }
                


            })
               .WithTags("Orders")
               .RequireAuthorization("ORDER_EDIT");

            app.MapGet("/orderDetailWithItems", async (int Id,IOrderRepository orderRepository) =>
            {
                try
                {
                    if (Id is 0)
                    {
                        return Results.BadRequest(ApiResponse<IEnumerable<OrderDetailsDto>>.Fail("OrderId is required"));
                    }

                    var result = await orderRepository.GetOrderDetailsAsync(Id);


                    if (result !=null)
                    {
                        
                            if (result.Image != null)
                            {
                                result.Image = Path.Combine(commonInfo.BaseUrl, commonInfo.CUSTOMER_IMG_PATH, result.Image); 
                            }

                        
                        return Results.Ok(ApiResponse<OrderDetailsDto>.Success(result));
                    }
                    return Results.Ok(ApiResponse<OrderDetailsDto>.Fail("No order details found"));

                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<OrderDetailsDto>.Fail("Internal Error!"));
                    throw;
                }
                

            })
            .WithTags("Orders")
            .RequireAuthorization("ORDER_VIEW"); ;
        }
    }
}

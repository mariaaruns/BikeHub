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

                    if (result.Data.Any())
                        return Results.Ok(ApiResponse<PagedResult<OrdersDto>>.Success(result));


                    return Results.Ok(ApiResponse<PagedResult<OrdersDto>>.Fail("No orders found"));
                
                }
                catch (Exception ex)
                {
                    
                    return Results.InternalServerError(ApiResponse<PagedResult<OrdersDto>>.Fail("Internal Server error, "+ex.Message));
                }
                

            })
            .WithTags("Orders")
            .WithName("GetOrders");


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
               

            }).WithTags("Orders");

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
                


            }).WithTags("Orders");

            app.MapGet("/orderDetailWithItems", async (int Id,IOrderRepository orderRepository) =>
            {
                try
                {
                    if (Id is 0)
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail("OrderId is required"));
                    }

                    var result = await orderRepository.GetOrderDetailsAsync(Id);


                    if (result.Any())
                        return Results.Ok(ApiResponse<IEnumerable<OrderDetailsDto>>.Success(result));

                    return Results.Ok(ApiResponse<IEnumerable<OrderDetailsDto>>.Fail("No order details found"));

                }
                catch (Exception)
                {

                    throw;
                }
                

            }).WithTags("Orders");
        }
    }
}

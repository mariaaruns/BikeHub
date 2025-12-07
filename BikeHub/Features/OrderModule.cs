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



        }
    }
}

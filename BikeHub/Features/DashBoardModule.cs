using BikeHub.Repository.IRepository;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Response;
using Carter;

namespace BikeHub.Features
{
    public class DashBoardModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/getcount", () =>
            {
                var obj = new DashboardResponseDto
                {
                    TotalProductsCount = 150,
                    TotalOrdersCount = 75,
                    TotalServiceCount = 12,
                    PendingServiceCount = 7,
                    CompletedServiceCount = 5
                };

                return Results.Ok(ApiResponse<DashboardResponseDto>.Success(obj));
            }).WithTags("Dashboard")
              .RequireAuthorization("AdminOnly");

            app.MapGet("/dashboardSalesAmount", async (int year,IDashboardRepository _dashboardRepository) =>
            {

                try
                {
                    if (year <= 0)
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail("Invalid year parameter."));
                    }

                    var result =await _dashboardRepository.GetSalesAmountByYearAsync(year);
                    
                    return Results.Ok(ApiResponse<IEnumerable<SalesAmountByYearDto>>.Success(result));
                }
                catch (Exception)
                {

                    throw;
                }
            }).WithTags("Dashboard");
        }
    }
}

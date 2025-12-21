using BikeHub.Repository.IRepository;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Response;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace BikeHub.Features
{
    public class DashBoardModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/getcount", async (DateTime date, [FromServices] IDashboardRepository _dashboardRepository) =>
            {

                try
                {
                    var result = await _dashboardRepository.GetCounts(date.Year, date.Month);

                    return Results.Ok(ApiResponse<DashboardResponseDto>.Success(result));
                }
                catch (Exception)
                {

                    throw;
                }


            }).WithTags("Dashboard");
          //    .RequireAuthorization("AdminOnly");

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

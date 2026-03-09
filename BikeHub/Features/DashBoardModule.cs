using BikeHub.Repository;
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
                    return Results.InternalServerError(ApiResponse<DashboardResponseDto>.Fail("Internal server error!"));

                }


            }).WithTags("Dashboard")
            .RequireAuthorization("DASHBOARD_VIEW");
            

            app.MapGet("/dashboardSalesAmount", async (int year,IDashboardRepository _dashboardRepository) =>
            {
                try
                {
                    if (year <= 0)
                    {
                        return Results.BadRequest(ApiResponse<IEnumerable<SalesAmountByYearDto>>.Fail("Invalid year parameter."));
                    }

                    var result =await _dashboardRepository.GetSalesAmountByYearAsync(year);
                    
                    return Results.Ok(ApiResponse<IEnumerable<SalesAmountByYearDto>>.Success(result));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<IEnumerable<SalesAmountByYearDto>>.Fail("Internal server error!"));

                }
            }).WithTags("Dashboard")
            .RequireAuthorization("DASHBOARD_VIEW");


            app.MapGet("/BrandYearlySales", async (int year, int orderStatus, [FromServices] IDashboardRepository _dashboardRepository) =>
            {

                try
                {
                    if (year <= 0)
                    {
                        return Results.BadRequest(ApiResponse<IEnumerable<BrandYearlySalesDto>>.Fail("Invalid year parameter."));
                    }

                    var result = await _dashboardRepository.GetBrandSalesByYearAsync(year, orderStatus);

                    return Results.Ok(ApiResponse<IEnumerable<BrandYearlySalesDto>>.Success(result));
                }
                catch (Exception ex)
                {

                    return Results.InternalServerError(ApiResponse<IEnumerable<BrandYearlySalesDto>>.Fail("Internal server error!"));
                }

            }).WithTags("Dashboard")
            .RequireAuthorization("DASHBOARD_VIEW");


        }
    }
}

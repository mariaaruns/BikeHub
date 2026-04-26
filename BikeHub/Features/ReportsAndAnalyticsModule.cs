using BikeHub.Service;
using BikeHub.Service.Interface;
using BikeHub.Shared.Common;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace BikeHub.Features
{
    public class ReportsAndAnalyticsModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/reports/ordersRevenue", async (DateTime fromdate, DateTime todate, [FromServices] IReportService _reportServices) =>
            {
                try
                {
                    if (fromdate == default || todate == default)
                        return Results.BadRequest("Invalid date range.");

                    var (isSuccess, Msg, filePath) = await _reportServices.CustomerOrderRevenue(fromdate, todate);

                    if (isSuccess)
                    {
                        //var fileBytes = await File.ReadAllBytesAsync(filePath);
                        //var fileName = Path.GetFileName(filePath);
                        //return Results.File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                        return Results.Ok(ApiResponse<string>.Success(filePath, Msg));
                    }
                    else
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail(Msg));
                    }
                }
                catch(Exception ex) {

                    return Results.InternalServerError(ApiResponse<string>.Fail(ex.Message));

                }

            }).WithTags("Reports & Analytics");

            app.MapPost("/api/reports/productRevenue", async (DateTime fromdate, DateTime todate, [FromServices] IReportService _reportServices) =>
            {
                try
                {
                    if (fromdate == default || todate == default)
                        return Results.BadRequest("Invalid date range.");

                    var (isSuccess, Msg, filePath) = await _reportServices.TopProductsByRevenue(fromdate, todate);

                    if (isSuccess)
                    {
                        //var fileBytes = await File.ReadAllBytesAsync(filePath);
                        //var fileName = Path.GetFileName(filePath);
                        //return Results.File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                        return Results.Ok(ApiResponse<string>.Success(filePath, Msg));
                    }
                    else
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail(Msg));
                    }
                }
                catch (Exception ex)
                {

                    return Results.InternalServerError(ApiResponse<string>.Fail(ex.Message));

                }
            }).WithTags("Reports & Analytics"); 


            app.MapPost("/api/reports/inventory", async ([FromServices] IReportService _reportServices) =>
            {
                try
                {
                  

                    var (isSuccess, Msg, filePath) = await _reportServices.Inventory();

                    if (isSuccess)
                    {
                        //var fileBytes = await File.ReadAllBytesAsync(filePath);
                        //var fileName = Path.GetFileName(filePath);
                        //return Results.File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                        return Results.Ok(ApiResponse<string>.Success(filePath, Msg));
                    }
                    else
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail(Msg));
                    }
                }
                catch (Exception ex)
                {

                    return Results.InternalServerError(ApiResponse<string>.Fail(ex.Message));

                }
            }).WithTags("Reports & Analytics"); 
            app.MapPost("/api/reports/bikeService", async (DateTime fromdate, DateTime todate, [FromServices] IReportService _reportServices) =>
            {
                try
                {
                    if (fromdate == default || todate == default)
                        return Results.BadRequest("Invalid date range.");

                    var (isSuccess, Msg, filePath) = await _reportServices.BikeServiceJobs(fromdate, todate);

                    if (isSuccess)
                    {
                        //var fileBytes = await File.ReadAllBytesAsync(filePath);
                        //var fileName = Path.GetFileName(filePath);
                        //return Results.File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                        return Results.Ok(ApiResponse<string>.Success(filePath, Msg));
                    }
                    else
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail(Msg));
                    }
                }
                catch (Exception ex)
                {

                    return Results.InternalServerError(ApiResponse<string>.Fail(ex.Message));

                }
            }).WithTags("Reports & Analytics"); ;
            app.MapPost("/api/reports/mechanicProductivity", async (DateTime date,[FromServices] IReportService _reportServices) =>
            {
                try
                {
                    if (date == default)
                        return Results.BadRequest("Invalid date range.");

                    var (isSuccess, Msg, filePath) = await _reportServices.MechanicProductivity(date);

                    if (isSuccess)
                    {
                        //var fileBytes = await File.ReadAllBytesAsync(filePath);
                        //var fileName = Path.GetFileName(filePath);
                        //return Results.File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                        return Results.Ok(ApiResponse<string>.Success(filePath, Msg));
                    }
                    else
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail(Msg));
                    }
                }
                catch (Exception ex)
                {

                    return Results.InternalServerError(ApiResponse<string>.Fail(ex.Message));

                }
            }).WithTags("Reports & Analytics"); 

        }
    }
}

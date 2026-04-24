using Carter;

namespace BikeHub.Features
{
    public class ReportsAndAnalyticsModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/reports/ordersRevenue", async (DateTime fromdate, DateTime todate) =>
            {

            })
                .WithTags("Reports & Analytics");

            app.MapPost("/api/reports/productRevenue", async (DateTime fromdate, DateTime todate) =>
            {

            }).WithTags("Reports & Analytics"); 


            app.MapPost("/api/reports/inventory", async (DateTime fromdate, DateTime todate) =>
            {

            }).WithTags("Reports & Analytics"); 
            app.MapPost("/api/reports/bikeService", async (DateTime fromdate, DateTime todate) =>
            {

            }).WithTags("Reports & Analytics"); ;
            app.MapPost("/api/reports/mechanicProductivity", async (DateTime fromdate, DateTime todate) =>
            {

            }).WithTags("Reports & Analytics"); 

        }
    }
}

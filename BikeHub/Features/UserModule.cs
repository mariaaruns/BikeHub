using Carter;

namespace BikeHub.Features
{
    public class UserModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/users", () => "User endpoint is working")
               .RequireAuthorization();

        }
    }
}

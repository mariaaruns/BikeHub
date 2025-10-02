using BikeHub.Repository.IRepository;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using Carter;
using System.Net;

namespace BikeHub.Features
{
    public class ProductsModule : ICarterModule
    {

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            //getAllProducts
            app.MapPost("/api/products", async (IProductRepository productRepository, GetProductsDto req) =>
            {
                try
                {
                    if (req.PageNumber is 0 || req.PageSize is 0) 
                    { 
                    return Results.BadRequest(ApiResponse<string>.Fail("Invalid Request", "PageNumber and PageSize must be greater than zero"));
                    }
                    var result = await productRepository.GetAllProductsAsync(req);
                    return Results.Ok(ApiResponse<PagedResult<ProductsDto>>.Success(result,"Products fetched successfully"));
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("Failed", ex.Message));
                }
                
            })
            .WithTags("Products")
            .Produces<ApiResponse<PagedResult<ProductsDto>>>(StatusCodes.Status200OK)
            .WithName("GetAllProducts");



        }
    }
}

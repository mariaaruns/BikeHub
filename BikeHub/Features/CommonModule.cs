using BikeHub.Repository.IRepository;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Response;
using Carter;
using Microsoft.AspNetCore.Mvc;
namespace BikeHub.Features
{
    public class CommonModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {

            app.MapGet("/api/Dropdown", async ([FromQuery]string type, [FromQuery]string? search, 
                [FromServices]IProductRepository productRepository, 
                [FromServices] IOrderRepository orderRepository, 
                [FromServices] ICustomerRepository customerRepository,
                IServiceRepository serviceRepository) =>
            {
                try
                {
                    if (string.IsNullOrEmpty(type))
                    {

                        return Results.BadRequest(ApiResponse<string>.Fail("Dropdown type is required"));
                    }

                    IEnumerable<DropdownDto> result;

                    switch (type.ToLower())
                    {

                        case "category":
                            result = await productRepository.DropDownCatgoryAsync();
                            break;
                        case "brand":
                            result = await productRepository.DropDownBrandAsync();
                            break;
                        case "orderstatus":
                             result=await orderRepository.GetOrderStatusDropdownAsync();
                            break;
                        case "servicestatus":
                            result = await serviceRepository.GetServiceStatusDropdownAsync();
                            break;
                        case "customer":
                            result = await customerRepository.GetCustomerDropdownAsync(search);
                            break;
                        default:
                            return Results.BadRequest(ApiResponse<string>.Fail("Invalid Type"));
                    }


                    if (result == null)
                    {
                        return Results.NotFound(ApiResponse<string>.Fail("Data was not Found"));
                    }

                    return Results.Ok(ApiResponse<IEnumerable<DropdownDto>>.Success(result, "Data Was Successfully Fetch"));
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("InternalServerError"));
                }
            }).WithTags("DropDownList")
            .Produces<ApiResponse<string>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError);


            app.MapGet("/api/products/stock-summaries", async (int brandId, int categoryId, [FromServices] IProductRepository respo) =>
            {
                try
                {
                    var result = await respo.DropDownProductAndStockAsync(brandId, categoryId);

                    if (result == null)
                    {
                        return Results.NotFound(ApiResponse<ProductDropdownDto>.Fail("Data was not found"));
                    }
                    return Results.Ok(ApiResponse<IEnumerable<ProductDropdownDto>>.Success(result, "Data was successfully fetched"));
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<ProductDropdownDto>.Fail("InternalServer error"));
                }

            }).WithTags("DropDownList")
              .Produces<ApiResponse<string>>(StatusCodes.Status400BadRequest)
              .Produces<ApiResponse<string>>(StatusCodes.Status404NotFound)
              .Produces<ApiResponse<IEnumerable<ProductDropdownDto>>>(StatusCodes.Status404NotFound);

        }
    }
}

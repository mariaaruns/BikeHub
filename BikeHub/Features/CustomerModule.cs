using BikeHub.Extension;
using BikeHub.Repository.IRepository;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using Carter;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BikeHub.Features
{
    public class CustomerModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/GetAllCustomers", async (ICustomerRepository Repo, ILogger<CustomerModule> logger, [FromBody]CustomerRequest req) =>
            {
                logger.LogInformation("/GetAllCustomers endpoint called at {Time}", DateTime.Now);
                try
                {
                    var result = await Repo.GetAllCustomerasync(req);

                    foreach (var item in result.Data.Where(x => !string.IsNullOrEmpty(x.Image)))
                    {
                        if (item.Image!.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                            continue;
                        item.Image= $"{commonInfo.BaseUrl}/{commonInfo.CUSTOMER_IMG_PATH}/{item.Image}";
                    }

                    if (result == null)
                    {
                        return Results.NotFound(ApiResponse<PagedResult<CustomersDto>>.Fail("Data Not Found"));
                    }

                    return Results.Ok(ApiResponse<PagedResult<CustomersDto>>.Success(result, "Data was fetch sucessfully"));

                }
                catch (Exception ex)
                {
                    logger.LogInformation("/GetAllCustomers endpoint failed at {Time}", DateTime.Now,"Exception:"+ex.Message);
                    return Results.InternalServerError(ApiResponse<PagedResult<CustomersDto>>.Fail("Internal Server Error"));
                }
                
            }).WithTags("Customers")
             .Produces<ApiResponse<CustomersDto>>(StatusCodes.Status404NotFound)
             .Produces<ApiResponse<CustomersDto>>(StatusCodes.Status200OK)
             .Produces<ApiResponse<CustomersDto>>(StatusCodes.Status500InternalServerError)
             .RequireAuthorization("CUSTOMER_VIEW");


            app.MapGet("GetCustomerById", async (ICustomerRepository customerRepository,int Id) =>
            {
                try
                {
                    
                    var result = await customerRepository.GetCustomerByIdAsync(Id);
                    if (result == null)
                    {
                        return Results.NotFound(ApiResponse<CustomerDetailDto>.Fail("no customer found"));
                    }

                    result.Image = $"{commonInfo.BaseUrl}/{commonInfo.CUSTOMER_IMG_PATH}/{result.Image}";

                    return Results.Ok(ApiResponse<CustomerDetailDto>.Success(result, "Data Was Successfully fetched"));

                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<CustomerDetailDto>.Fail("InternalServer Error"));
                }
            }).WithTags("Customers")
             .Produces<ApiResponse<CustomerDetailDto>>(StatusCodes.Status404NotFound)
             .Produces<ApiResponse<CustomerDetailDto>>(StatusCodes.Status200OK)
             .Produces<ApiResponse<CustomerDetailDto>>(StatusCodes.Status500InternalServerError)
             .RequireAuthorization("CUSTOMER_VIEW");

            app.MapPost("/AddCustomer", async (ICustomerRepository CusRepo, [FromBody]AddCustomerDto req) =>
            {
                try
                {     
                    var extension = ImageHelper.GetImageExtension(req.Imagebyte);

                    if (string.IsNullOrEmpty(extension))
                    {
                        throw new Exception("Invalid image format");
                    }

                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), commonInfo.CUSTOMER_IMG_PATH);


                    var fileName = await ImageHelper.SaveImageAsync(req.Imagebyte,
                                                folderPath,
                                                req.FirstName,
                                                extension);
                    req.ImageUrl = fileName;

                    var insertedId = await CusRepo.AddCustomerAsync(req);
                    return Results.Ok(ApiResponse<int>.Success(insertedId,"Add The Data successfully"));
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<int>.Fail("Internal Server Error"));
                }
            }).WithTags("Customers").DisableAntiforgery()
            .Produces<ApiResponse<int>>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse<int>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<int>>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization("CUSTOMER_ADD");

            app.MapPut("/UpdateCustomer", async (ICustomerRepository customerRepository, [FromBody] UpdateCustomerDto req) =>
            {
                try
                {
                    var IsValid = ModelValidator.Validate(req);

                    if (!IsValid.IsValid)
                    {
                        return Results.BadRequest(ApiResponse<bool>.Fail("Invalid Request", IsValid.Errors));
                    }

                    var existingCustomer = await customerRepository.GetCustomerByIdAsync(req.Id);

                    if (existingCustomer == null)
                    {
                        return Results.NotFound(ApiResponse<bool>.Fail("Customer Not Found"));
                    }

                    var extension = ImageHelper.GetImageExtension(req.Imagebyte);

                    if (!string.IsNullOrEmpty(extension))
                    {
                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), commonInfo.CUSTOMER_IMG_PATH);


                        var fileName = await ImageHelper.SaveImageAsync(req.Imagebyte,
                                                    folderPath,
                                                    req.FirstName,
                                                    extension);
                        req.ImageUrl = fileName;
                    }
                    else 
                    {
                        req.ImageUrl = existingCustomer.Image; 
                    }
                    bool isSucess = await customerRepository.UpdateCustomerAsync(req);
                    return Results.Ok(ApiResponse<bool>.Success(isSucess,"Data was successfully Modifed"));
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<bool>.Fail("InternalServer Isuess"));
                }
            }).WithTags("Customers").DisableAntiforgery()
            .Produces<ApiResponse<bool>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<bool>>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization("CUSTOMER_EDIT");


            app.MapDelete("DeActivateCustomer/{id:int}", async (ICustomerRepository customerrepository, [FromRoute]int Id) =>
            {
                try
                {
                    await customerrepository.DeActivateCustomerAsync(Id);

                    return Results.Ok(ApiResponse<string>.Success("Data Was Successfully DeActive"));
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("InternalServer Isuess"));
                }
            }).WithTags("Customers")
            .Produces<ApiResponse<string>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization("CUSTOMER_DELETE");
        }
    }
}

using BikeHub.Repository;
using BikeHub.Repository.IRepository;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using Carter;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BikeHub.Features
{
    public class ProductsModule : ICarterModule
    {

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            //getAllProducts
            app.MapPost("/products", async (IProductRepository productRepository, GetProductsDto req) =>
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
            .Produces<ApiResponse<string>>(StatusCodes.Status400BadRequest)
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError)
            .WithName("GetAllProducts");



            app.MapPost("/products/add", async (IProductRepository productRepository,[FromForm] AddProductsDto req) => {

                string newfilePath = string.Empty;
                try
                {
                    var IsValid = ModelValidator.Validate(req);

                    if (!IsValid.IsValid)
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail("Invalid Request", IsValid.Errors));
                    }

                    if (req.ProductImage != null && req.ProductImage.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), commonInfo.PRODUCT_IMG_PATH);
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(req.ProductImage.FileName)}";
                         newfilePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(newfilePath, FileMode.Create))
                        {
                            await req.ProductImage.CopyToAsync(stream);
                        }

                        req.ImageUrl = $"{fileName}";
                    }
                  
                    var result = await productRepository.CreateProductAsync(req);
                    
                    if(result == 0)
                    {
                        if (File.Exists(newfilePath))
                        {
                            File.Delete(newfilePath);
                        }
                        return Results.BadRequest(ApiResponse<string>.Fail("Failed", "Product creation failed"));
                        
                    }
                    
                    return Results.Ok(ApiResponse<int>.Success(result, "Product created successfully"));

                }
                catch (Exception ex)
                {
                    if (File.Exists(newfilePath))
                    {
                        File.Delete(newfilePath);
                    }

                    return Results.InternalServerError(ApiResponse<string>.Fail("Failed", ex.Message));
                }

            })
            .WithTags("Products")
            .Produces<ApiResponse<int>>(StatusCodes.Status200OK)
            .DisableAntiforgery()
            .WithName("Create Product");


            
            app.MapPut("/products/update", async (IProductRepository productRepository, [FromForm] UpdateProductDto req) =>
            {
                string newFilePath = string.Empty;

                try
                {
                    var IsValid = ModelValidator.Validate(req);
                    if (!IsValid.IsValid)
                        return Results.BadRequest(ApiResponse<string>.Fail("Invalid Request", IsValid.Errors));

                    if (req.ProductId <= 0)
                        return Results.BadRequest(ApiResponse<string>.Fail("Invalid ProductId"));

                    //get existing product details
                    var productDetails = await productRepository.GetProductByIdAsync(req.ProductId);
                    if (productDetails == null)
                        return Results.NotFound(ApiResponse<string>.Fail("Product not found"));

                    //  Save new image if provided
                    if (req.ProductImage != null && req.ProductImage.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), commonInfo.PRODUCT_IMG_PATH);
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(req.ProductImage.FileName)}";
                        newFilePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(newFilePath, FileMode.Create))
                        {
                            await req.ProductImage.CopyToAsync(stream);
                        }

                        req.ImageUrl = fileName;
                    }
                    else
                    {
                        req.ImageUrl = productDetails.ProductImage; // keep old image
                    }

                    //Update DB
                    var result = await productRepository.UpdateProductByIdAsync(req);

                    if (!result)
                    {
                        // delete if file created and db not updated
                        if (!string.IsNullOrEmpty(newFilePath) && File.Exists(newFilePath))
                            File.Delete(newFilePath);

                        return Results.BadRequest(ApiResponse<string>.Fail("Failed", "Product update failed"));
                    }

                    //  Delete old image only after successful DB update
                    if (req.ProductImage != null && !string.IsNullOrEmpty(productDetails.ProductImage))
                    {
                        var oldFilePath = Path.Combine(commonInfo.PRODUCT_IMG_PATH, productDetails.ProductImage);
                        if (File.Exists(oldFilePath))
                            File.Delete(oldFilePath);
                    }

                    return Results.Ok(ApiResponse<bool>.Success(true, "Product updated successfully"));
                }
                catch (Exception ex)
                {
                    // cleanup new file if something went wrong
                    if (!string.IsNullOrEmpty(newFilePath) && File.Exists(newFilePath))
                        File.Delete(newFilePath);

                    return Results.Problem(ApiResponse<string>.Fail("Failed", ex.Message).ToString(), statusCode: 500);
                }
            })
              .WithTags("Products")
              .Produces<ApiResponse<bool>>(StatusCodes.Status200OK)
              .DisableAntiforgery()
              .WithName("Update Product");

            app.MapGet("/products/{id}", async (IProductRepository productRepository, [FromRoute, Required] int id) =>
            {
                try
                {
                    if (id <= 0)
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail("Invalid Request", "Id must be greater than zero"));
                    }
                    var result = await productRepository.GetProductByIdAsync(id);
                    if(result == null)
                    {
                        return Results.NotFound(ApiResponse<string>.Fail("Not Found", "Product not found"));
                    }
                    return Results.Ok(ApiResponse<GetProductByIdDto>.Success(result, "Product fetched successfully"));
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("Failed", ex.Message));
                }
            })
            .WithTags("Products")
            .Produces<ApiResponse<GetProductByIdDto>>(StatusCodes.Status200OK)
            .WithName("Get Product");



            app.MapPatch("/products/{id}/deactivate", async (IProductRepository repo, int id) =>
            {
                var result = await repo.DeactivateProductAsync(id);
                return result
                    ? Results.Ok(ApiResponse<bool>.Success(true, "Product deactivated"))
                    : Results.NotFound(ApiResponse<string>.Fail("Product not found"));
            }).WithTags("Products")
            .Produces<ApiResponse<bool>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<string>>(StatusCodes.Status404NotFound)
            .WithName("Deactivate Product");




            //category start


        }
    }
}

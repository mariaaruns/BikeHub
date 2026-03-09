using BikeHub.DapperQuery;
using BikeHub.Extension;
using BikeHub.Repository;
using BikeHub.Repository.IRepository;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Runtime.InteropServices;

namespace BikeHub.Features
{
    public class ProductsModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            //Products Start

            app.MapPost("/products", async (IProductRepository productRepository, GetProductsDto req) =>
            {
                try
                {
                    if (req.PageNumber is 0 || req.PageSize is 0)
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail("Invalid Request", "PageNumber and PageSize must be greater than zero"));
                    }

                    var result = await productRepository.GetAllProductsAsync(req);

                    foreach (var item in result.Data.Where(x=>!string.IsNullOrEmpty(x.ProductImage))) 
                    {
                        if (item.ProductImage!.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                            continue;

                        item.ProductImageUrl = $"{commonInfo.BaseUrl}/{commonInfo.PRODUCT_IMG_PATH}/{item.ProductImage}";
                    }

                    return Results.Ok(ApiResponse<PagedResult<ProductsDto>>.Success(result, "Products fetched successfully"));

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
            .WithName("GetAllProducts")
            .RequireAuthorization("PRODUCT_VIEW");

            app.MapPost("/products/add", async (IProductRepository productRepository, [FromBody] AddProductsDto req) =>
            {

                string newfilePath = string.Empty;
                try
                {
                    var IsValid = ModelValidator.Validate(req);

                    if (!IsValid.IsValid)
                    {
                        return Results.BadRequest(ApiResponse<int>.Fail("Invalid Request", IsValid.Errors));
                    }

                    var extension = ImageHelper.GetImageExtension(req.Imagebyte);

                    if (string.IsNullOrEmpty(extension))
                    {
                        throw new Exception("Invalid image format");
                    }

                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), commonInfo.PRODUCT_IMG_PATH);


                    var fileName = await ImageHelper.SaveImageAsync(req.Imagebyte,
                                                folderPath,
                                                req.ProductName,
                                                extension);
                    req.ImageUrl = fileName;


                    var result = await productRepository.CreateProductAsync(req);

                    if (result == 0)
                    {
                        return Results.BadRequest(ApiResponse<int>.Fail("Failed", "Product creation failed"));
                    }

                    return Results.Ok(ApiResponse<int>.Success(result, "Product created successfully"));

                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<int>.Fail("Failed", ex.Message));
                }

            })
            .WithTags("Products")
            .Produces<ApiResponse<int>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<int>>(StatusCodes.Status500InternalServerError)
            .Produces<ApiResponse<int>>(StatusCodes.Status400BadRequest)
            .DisableAntiforgery()
            .WithName("Create Product").RequireAuthorization("PRODUCT_ADD"); ;


            app.MapPut("/products/update", async (IProductRepository productRepository, [FromForm] UpdateProductDto req) =>
            {
                try
                {
                    var IsValid = ModelValidator.Validate(req);
                    if (!IsValid.IsValid)
                        return Results.BadRequest(ApiResponse<bool>.Fail("Invalid Request", IsValid.Errors));

                    if (req.ProductId <= 0)
                        return Results.BadRequest(ApiResponse<bool>.Fail("Invalid ProductId"));

                    //get existing product details
                    var productDetails = await productRepository.GetProductByIdAsync(req.ProductId);
                    if (productDetails == null)
                        return Results.NotFound(ApiResponse<bool>.Fail("Product not found"));

                    //  Save new image if provided
                   
                    var extension = ImageHelper.GetImageExtension(req.Imagebyte);

                    if (!string.IsNullOrEmpty(extension))
                    {
                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), commonInfo.PRODUCT_IMG_PATH);


                        var fileName = await ImageHelper.SaveImageAsync(req.Imagebyte,
                                                    folderPath,
                                                    req.ProductName,
                                                    extension);
                        req.ImageUrl = fileName;
                    }
                    else
                    {
                        req.ImageUrl = productDetails.ProductImage;
                    }

                    //Update DB
                    var result = await productRepository.UpdateProductByIdAsync(req);

                    if (!result)
                    {
                        return Results.BadRequest(ApiResponse<bool>.Fail("Failed", "Product update failed"));
                    }
                    return Results.Ok(ApiResponse<bool>.Success(true, "Product updated successfully"));
                }
                catch (Exception ex)
                {
                    return Results.Problem(ApiResponse<bool>.Fail("Failed", ex.Message).ToString(), statusCode: 500);
                }
            })
              .WithTags("Products")
              .Produces<ApiResponse<bool>>(StatusCodes.Status200OK)
              .DisableAntiforgery()
              .WithName("Update Product").RequireAuthorization("PRODUCT_EDIT"); 


            app.MapGet("/products/{id}", async (IProductRepository productRepository, [FromRoute, Required] int id) =>
            {
                try
                {
                    if (id <= 0)
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail("Invalid Request", "Id must be greater than zero"));
                    }
                    var result = await productRepository.GetProductByIdAsync(id);

                    if (result == null)
                    {
                        return Results.NotFound(ApiResponse<string>.Fail("Not Found", "Product not found"));
                    }
                    result.ProductImage=!string.IsNullOrEmpty(result.ProductImage) && !result.ProductImage.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                        ? $"{commonInfo.BaseUrl}/{commonInfo.PRODUCT_IMG_PATH}/{result.ProductImage}"
                        : result.ProductImage;
                    return Results.Ok(ApiResponse<GetProductByIdDto>.Success(result, "Product fetched successfully"));
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("Failed", ex.Message));
                }
            })
            .WithTags("Products")
            .DisableAntiforgery()
            .Produces<ApiResponse<GetProductByIdDto>>(StatusCodes.Status200OK)
            .WithName("Get Product")
            .RequireAuthorization("PRODUCT_VIEW");

            app.MapPatch("/products/{id}/deactivate", async (IProductRepository repo, int id) =>
            {
                var result = await repo.DeactivateProductAsync(id);
                return result
                    ? Results.Ok(ApiResponse<bool>.Success(true, "Product deactivated"))
                    : Results.NotFound(ApiResponse<bool>.Fail("Product not found"));
            }).WithTags("Products")
            .DisableAntiforgery()
            .Produces<ApiResponse<bool>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<bool>>(StatusCodes.Status404NotFound)
            .WithName("Deactivate Product")
            .RequireAuthorization("PRODUCT_DELETE"); ;


            //category start
            app.MapPost("/categoryAdd", async (IProductRepository repos, AddCategoryDto dto) =>
            {
                try
                {
                    await repos.CreateCategoryAsync(dto);

                    return Results.Ok(ApiResponse<string>.Success("Category add Successfully"));

                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("Faild", ex.Message));
                }
            }).WithTags("Category")
            .DisableAntiforgery()
            .Produces<ApiResponse<string>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError)
            .WithName("CategoryProduct").RequireAuthorization("PRODUCT_ADD"); ;

            app.MapGet("/GetCategory", async (IProductRepository repo,string? CategoryNameFilter) =>
            {
                try
                {
                    var result = await repo.GetAllCategoryAsync(CategoryNameFilter);
                    if (result == null)
                    {
                        return Results.NotFound(ApiResponse<IEnumerable<CategoryDto>>.Fail("Data Not Found"));
                    }
                    return Results.Ok(ApiResponse<IEnumerable<CategoryDto>>.Success(result));

                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<IEnumerable<CategoryDto>>.Fail("Internal Server Error"));
                }
            }).WithTags("Category")
            .DisableAntiforgery()
            .Produces<ApiResponse<IEnumerable<CategoryDto>>>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse<IEnumerable<CategoryDto>>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<IEnumerable<CategoryDto>>>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization("PRODUCT_VIEW"); 

            app.MapGet("/GetCategoryById", async (IProductRepository product, int id) =>
            {
                try
                {
                    var FinalResults = await product.GetCategoryByIdAsync(id);

                    if (FinalResults == null)
                    {
                        return Results.NotFound(ApiResponse<CategoryDto>.Fail("Data Not Found"));
                    }

                    return Results.Ok(ApiResponse<CategoryDto>.Success(FinalResults, "Data Was Successfully Fetched"));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<CategoryDto>.Fail("Internal Server Error"));
                }
            }).WithTags("Category")
            .DisableAntiforgery()
            .Produces<ApiResponse<CategoryDto>>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse<CategoryDto>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<CategoryDto>>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization("PRODUCT_VIEW"); ;

            app.MapPut("/UpdateCategory", async (IProductRepository Ipo, UpdateCategoryDto upt) =>
            {
                try
                {
                    await Ipo.UpdateCategoryByIdAsync(upt);

                    return Results.Ok(ApiResponse<string>.Success("Data Was Update Successfully"));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("Internal Server Error"));
                }
            }).WithTags("Category")
            .DisableAntiforgery()
            .Produces<ApiResponse<string>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization("PRODUCT_EDIT"); 

            app.MapDelete("/DeleteCategory", async (IProductRepository ipo, int id) =>
            {
                try
                {
                    await ipo.DeleteCategoryByIdAsync(id);
                    return Results.Ok(ApiResponse<string>.Success("Data Was Successfully Removed"));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("Internal Server Error"));
                }
            }).WithTags("Category")
            .DisableAntiforgery()
            .Produces<ApiResponse<string>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization("PRODUCT_DELETE");




            //brand start
            app.MapPost("/AddBrand", async (IProductRepository _productRepository, [FromForm] AddBrandDto dto) =>
            {
                var fileNewPath = string.Empty;
                try
                {
                    var Isvalid = ModelValidator.Validate(dto);
                    if (!Isvalid.IsValid)
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail("Invalid request", Isvalid.Errors));
                    }

                    var extension = ImageHelper.GetImageExtension(dto.Imagebyte);

                    if (string.IsNullOrEmpty(extension))
                    {
                        throw new Exception("Invalid image format");
                    }

                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), commonInfo.Brand_IMG_PATH);


                    var fileName = await ImageHelper.SaveImageAsync(dto.Imagebyte,
                                                folderPath,
                                                dto.BrandName,
                                                extension);
                    dto.ImageUrl = fileName;

                    await _productRepository.CreateBrandAsync(dto);

                    return Results.Ok(ApiResponse<string>.Success("Data Was Successfully"));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("Data Was Not Insert"));
                }
            }).WithTags("Brand")
            .DisableAntiforgery()
            .Produces<ApiResponse<string>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization("PRODUCT_ADD"); ;

             app.MapPut("/updateBrand", async (IProductRepository _productRepository, [FromForm] UpdateBrandDto dto) =>
            {
                var fileNewPath = string.Empty;
                try
                {
                    var Isvalid = ModelValidator.Validate(dto);
                    
                    if (!Isvalid.IsValid)
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail("Invalid request", Isvalid.Errors));
                    }

                    var existingBrand = await _productRepository.GetBrandByIdAsync(dto.BrandId);
                    
                    if (existingBrand == null)
                    {
                        return Results.NotFound(ApiResponse<string>.Fail("Brand not found"));
                    }


                    var extension = ImageHelper.GetImageExtension(dto.Imagebyte);

                    if (!string.IsNullOrEmpty(extension))
                    {
                        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), commonInfo.Brand_IMG_PATH);


                        var fileName = await ImageHelper.SaveImageAsync(dto.Imagebyte,
                                                    folderPath,
                                                    dto.BrandName,
                                                    extension);
                        dto.ImageUrl = fileName;
                    }
                    else {
                    
                    dto.ImageUrl = existingBrand.Image;

                    }
                        //  await _productRepository.Bran(dto);
                        return Results.Ok(ApiResponse<string>.Success("Brand Updated Successfully"));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("Data Was Not Updated!!!"));
                }
            }).WithTags("Brand")
            .DisableAntiforgery()
            .Produces<ApiResponse<string>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization("PRODUCT_EDIT");




            app.MapGet("/GetAllBrand", async (IProductRepository respo,string? BrandNameFilter) =>
            {
                try
                {
                    var rowsaffect = await respo.GetAllBrandsAsync(BrandNameFilter);
                    rowsaffect = rowsaffect.Select(x => new BrandsDto
                    {
                        BrandId=x.BrandId,
                        BrandName=x.BrandName,
                        Image= @"https://f405rch9-7079.inc1.devtunnels.ms/Content/Images/Brands/" + x.Image
                    });
                    if (rowsaffect == null)
                    {
                        return Results.NotFound(ApiResponse<IEnumerable<string>>.Fail("Data Not Found"));
                    }
                    return Results.Ok(ApiResponse<IEnumerable<BrandsDto>>.Success(rowsaffect));

                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<IEnumerable<string>>.Fail("InternalServerError"));
                }
            }).WithTags("Brand")
            .DisableAntiforgery()
            .Produces<ApiResponse<BrandsDto>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<string>>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization("PRODUCT_VIEW");

            app.MapGet("/GetBrandById", async (IProductRepository repos, int Id) =>
            {
                try
                {
                    var result = await repos.GetBrandByIdAsync(Id);

                    if (result == null)
                    {
                        return Results.NotFound(ApiResponse<string>.Fail("Data Was Not Found"));
                    }

                    return Results.Ok(ApiResponse<BrandsDto>.Success(result));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("InternalServerError"));
                }
            }).WithTags("Brand")
            .DisableAntiforgery()
            .Produces<ApiResponse<BrandsDto>>(StatusCodes.Status404NotFound)
            .Produces<ApiResponse<BrandsDto>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<BrandsDto>>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization("PRODUCT_VIEW"); ;

            app.MapPut("/DeleteBrandById", async (IProductRepository repos, int Id) =>
            {
                try
                {
                    await repos.DeleteBrandByIdAsync(Id);

                    return Results.Ok(ApiResponse<string>.Success("Data Was Deactive Successfully"));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("InternalServerError"));
                }
            }).WithTags("Brand")
            .DisableAntiforgery()
            .Produces<ApiResponse<string>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization("PRODUCT_DELETE"); ;


        }

    }
}

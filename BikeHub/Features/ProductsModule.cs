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
            .WithName("GetAllProducts").RequireAuthorization();



            app.MapPost("/products/add", async (IProductRepository productRepository, [FromForm] AddProductsDto req) =>
            {

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

                    if (result == 0)
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
                    if (result == null)
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
            .DisableAntiforgery()
            .Produces<ApiResponse<GetProductByIdDto>>(StatusCodes.Status200OK)
            .WithName("Get Product");



            app.MapPatch("/products/{id}/deactivate", async (IProductRepository repo, int id) =>
            {
                var result = await repo.DeactivateProductAsync(id);
                return result
                    ? Results.Ok(ApiResponse<bool>.Success(true, "Product deactivated"))
                    : Results.NotFound(ApiResponse<string>.Fail("Product not found"));
            }).WithTags("Products")
            .DisableAntiforgery()
            .Produces<ApiResponse<bool>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<string>>(StatusCodes.Status404NotFound)
            .WithName("Deactivate Product");





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
            .WithName("CategoryProduct");

            app.MapGet("GetCategory", async (IProductRepository repo,string? CategoryNameFilter) =>
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
            .Produces<ApiResponse<IEnumerable<CategoryDto>>>(StatusCodes.Status500InternalServerError);

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
            .Produces<ApiResponse<CategoryDto>>(StatusCodes.Status500InternalServerError);

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
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError);

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
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError);




            //brand start
            app.MapPost("/AddBrand", async (IProductRepository Ipo, [FromForm] AddBrandDto dto) =>
            {
                var fileNewPath = string.Empty;
                try
                {
                    var Isvalid = ModelValidator.Validate(dto);
                    if (!Isvalid.IsValid)
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail("Invalid request", Isvalid.Errors));
                    }
                    if (dto.BrandImage != null && dto.BrandImage.Length > 0)
                    {
                        var uploadFilse = Path.Combine(Directory.GetCurrentDirectory(), commonInfo.Brand_IMG_PATH);

                        if (!Directory.Exists(uploadFilse))
                            Directory.CreateDirectory(uploadFilse);

                        var folderpath = $"{Guid.NewGuid()}{Path.GetExtension(dto.BrandImage.FileName)}";
                        fileNewPath = Path.Combine(uploadFilse, folderpath);

                        using (var stream = new FileStream(fileNewPath, FileMode.Create))
                        {
                            await dto.BrandImage.CopyToAsync(stream);
                        }
                        dto.ImageUrl = folderpath;
                    }
                    await Ipo.CreateBrandAsync(dto);

                    return Results.Ok(ApiResponse<string>.Success("Data Was Successfully"));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("Data Was Not Insert"));
                }
            }).WithTags("Brand")
            .DisableAntiforgery()
            .Produces<ApiResponse<string>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError);


            app.MapGet("/GetAllBrand", async (IProductRepository respo,string? BrandNameFilter) =>
            {
                try
                {
                    var rowsaffect = await respo.GetAllBrandsAsync(BrandNameFilter);

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
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError);

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
            .Produces<ApiResponse<BrandsDto>>(StatusCodes.Status500InternalServerError);

            app.MapPut("/DeleteByIdBrand", async (IProductRepository repos, int Id) =>
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
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError);

            app.MapGet("/DropDownCategory", async (IProductRepository respo) =>
            {
                try
                {
                    var rowsffect = await respo.DropDownCatgoryAsync();
                    if(rowsffect == null)
                    {
                        return Results.NotFound(ApiResponse<string>.Fail("Data was not Found"));
                    }
                    return Results.Ok(ApiResponse<IEnumerable<ProductDto1>>.Success(rowsffect,"Data Was Successfully Fetch"));
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("InternalServerError"));
                }
            }).WithTags("DropDownList")
            .Produces<ApiResponse<string>>(StatusCodes.Status200OK)
            .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError);


            app.MapGet("BrandDropDown", async (IProductRepository respo) =>
            {
                try
                {
                    var rowsaffect = await respo.DropDownBrandAsync();

                    if (rowsaffect == null)
                    {
                        return Results.NotFound(ApiResponse<string>.Fail("Data was not found"));
                    }
                    return Results.Ok(ApiResponse<IEnumerable<BrandsDto1>>.Success(rowsaffect, "Row Successfull fetched"));
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("InternalError Issuses"));
                }
            });

            app.MapGet("StockDropDown", async (IProductRepository respo) =>
            {
                try
                {
                    var rowsaffect = await respo.DropDownStockAsync();

                    if (rowsaffect==null)
                    {
                        return Results.NotFound(ApiResponse<string>.Fail("Data was not found"));
                    }
                    return Results.Ok(ApiResponse<IEnumerable<productDto2>>.Success(rowsaffect,"Data was successfully fetched"));
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("InternalServer Issuses"));
                }               
            });
            

            
        }

    }
}

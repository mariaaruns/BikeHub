

using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using res = BikeHub.Shared.Common;
namespace BikeHub.Mobile.ApiServices
{
   public interface IProductApi
    {

        //product consumes here
        [Post("/products")]
        Task<res.ApiResponse<res.PagedResult<ProductsDto>>> GetProductsAsync(GetProductsDto dto, CancellationToken cancellationToken);

        [Post("/products/add")]
        Task<res.ApiResponse<string>> AddProductAsync(AddProductsDto dto, CancellationToken cancellationToken);

        [Get("/products/{id}")]
        Task<res.ApiResponse<GetProductByIdDto>> GetProductByIdAsync(int id, CancellationToken cancellationToken);

        [Put("/products/update")]
        Task<res.ApiResponse<string>> UpdateProductAsync(UpdateProductDto dto, CancellationToken cancellationToken);

        [Patch("/products/{id}/deactivate")]
        Task<res.ApiResponse<string>> DeactivateProductAsync(int id, CancellationToken cancellationToken);



        //category consumes here

        [Get("/GetCategory")]
        Task<res.ApiResponse<IEnumerable<CategoryDto>>> GetAllCategoriesAsync(string? BrandNameFilter, CancellationToken cancellationToken);

        [Post("/categoryadd")]
        Task<res.ApiResponse<string>> AddCategoryAsync(AddCategoryDto dto, CancellationToken cancellationToken);

        [Delete("/DeleteCategory")]
        Task<res.ApiResponse<string>> DeleteCategoryByIdAsync(int Id, CancellationToken cancellationToken);

        [Put("/UpdateCategory")]
        Task<res.ApiResponse<string>> UpdateCategoryAsync(UpdateCategoryDto dto, CancellationToken cancellationToken);
        
        [Get("/GetCategoryById")]
        Task<res.ApiResponse<CategoryDto>> GetCategoryByIdAsync(int Id, CancellationToken cancellationToken);


        //brand consumes here

        [Get("/GetAllBrand")]
        Task<res.ApiResponse<IEnumerable<BrandsDto>>> GetAllBrandsAsync(string? BrandNameFilter, CancellationToken cancellationToken);

        [Post("/AddBrand")]
        Task<res.ApiResponse<string>> AddBrandAsync(AddBrandDto dto, CancellationToken cancellationToken);
        
        [Get("/GetBrandById")]
        Task<res.ApiResponse<BrandsDto>> GetBrandByIdAsync(int Id, CancellationToken cancellationToken);

        [Delete("/DeleteBrandById")]
        Task<res.ApiResponse<string>> DeleteBrandByIdAsync(int Id, CancellationToken cancellationToken);


        //dropdown consumes here
        [Get("/Dropdown")]
        Task<res.ApiResponse<IEnumerable<DropdownDto>>> GetDropdownAsync(string type,CancellationToken cancellationToken);

        [Get("/ProductAndStockDropDown")]
        Task<res.ApiResponse<IEnumerable<ProductDropdownDto>>> GetProductAndStockDropDownAsync(CancellationToken cancellationToken);
    }
}

using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BikeHub.Repository.IRepository
{
    public interface IProductRepository
    {
        //brands
        Task CreateBrandAsync(AddBrandDto BRes);
        Task<IEnumerable<BrandsDto>> GetAllBrandsAsync();
        Task<BrandsDto> GetBrandByIdAsync(int Id);
        
        //Task UpdateBrandByIdAsync();
        Task DeleteBrandByIdAsync(int Id);

        //category
        Task CreateCategoryAsync(AddCategoryDto dto);
        Task<IEnumerable<CategoryDto>> GetAllCategoryAsync();
        Task<CategoryDto> GetCategoryByIdAsync(int Id);
        Task UpdateCategoryByIdAsync(UpdateCategoryDto upt);
        Task DeleteCategoryByIdAsync(int Id);

        //product
        Task<int> CreateProductAsync(AddProductsDto dto);
        Task<PagedResult<ProductsDto>> GetAllProductsAsync(GetProductsDto dto);
        Task<GetProductByIdDto> GetProductByIdAsync(int id);
        Task<bool> UpdateProductByIdAsync(UpdateProductDto dto);
        Task<bool> DeactivateProductAsync(int id);

        
    }
}

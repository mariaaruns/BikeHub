using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;

namespace BikeHub.Repository.IRepository
{
    public interface IProductRepository
    {
        //brands
        //Task CreateBrandAsync();
        //Task GetAllBrandsAsync();
        //Task GetBrandsByIdAsync();
        //Task UpdateBrandByIdAsync();
        //Task DeleteBrandByIdAsync();

        //category
        //Task CreateCategoryAsync();
        //Task GetAllCategoriesAsync();
        //Task GetCategoryByIdAsync();
        //Task UpdateCategoryByIdAsync();
        //Task DeleteCategoryByIdAsync();


        //product

        Task<int> CreateProductAsync(AddProductsDto dto);
        Task<PagedResult<ProductsDto>> GetAllProductsAsync(GetProductsDto dto);
        Task<GetProductByIdDto> GetProductByIdAsync(int id);
        Task<bool> UpdateProductByIdAsync(UpdateProductDto dto);
        Task<bool> DeactivateProductAsync(int id);

        
    }
}

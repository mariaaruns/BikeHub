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

        //Task CreateProductAsync();
        Task<PagedResult<ProductsDto>> GetAllProductsAsync(GetProductsDto dto);
        //Task GetProductByIdAsync();
        //Task UpdateProductByIdAsync();
        //Task DeleteProductByIdAsync();

        
    }
}

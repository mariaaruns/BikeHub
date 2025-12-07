using BikeHub.DapperQuery;
using BikeHub.Repository.IRepository;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Data;

namespace BikeHub.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _connection;

        public ProductRepository(IDbConnection connection)
        {
            this._connection = connection;
        }

        public async Task<int> CreateProductAsync(AddProductsDto dto)
        {

            var sql = ProductQuery.CreateProduct;
            var StockSql = ProductQuery.AddProductStock;
            int LastInsertId = 0;

            var parameters = new
            {
                @productName = dto.ProductName,
                @brandId = dto.BrandId,
                @categoryId = dto.CategoryId,
                @modelyear = dto.ModelYear,
                @listPrice = dto.ListPrice,
                @productImage = dto.ImageUrl,
            };


            using (var connection = new SqlConnection(_connection.ConnectionString)) 
            {
                await connection.OpenAsync();

                var transaction = await connection.BeginTransactionAsync();
                try
                {
                    LastInsertId = await connection.ExecuteScalarAsync<int>(sql, parameters, transaction: transaction);
                    //add stock for newly added product

                    var Stockparameters = new
                    {
                        @productId = LastInsertId,
                        @stockQty = dto.StockQty
                    };

                    await connection.ExecuteAsync(StockSql, Stockparameters, transaction: transaction);

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return LastInsertId;
        }

        public Task<PagedResult<ProductsDto>> GetAllProductsAsync(GetProductsDto dto)
        {
            try
            {
                var sql = ProductQuery.GetProducts;
                var TotalcountSql = ProductQuery.TotalProductsCount;

                int TotalRecorscount = 0;
                var productsList = new List<ProductsDto>();

                var parameters = new
                {
                    Search = string.IsNullOrEmpty(dto.ProductNameFilter) ? null : $"%{dto.ProductNameFilter}%",
                    Offset = (dto.PageNumber - 1) * dto.PageSize,
                    PageSize = dto.PageSize
                };

                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    productsList = connection.Query<ProductsDto>(sql, parameters).ToList();
                    TotalRecorscount = connection.ExecuteScalar<int>(TotalcountSql, parameters);
                }

                var PagedResult = new PagedResult<ProductsDto>(TotalRecorscount,
                    dto.PageNumber,
                    dto.PageSize,
                    productsList);

                return Task.FromResult(PagedResult);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<GetProductByIdDto> GetProductByIdAsync(int id)
        {
            var sql = ProductQuery.GetProduct;
            var product = new GetProductByIdDto();

            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    product = await connection.QueryFirstOrDefaultAsync<GetProductByIdDto>(
                    sql,
                    new { @id = id });
                }
            }
            catch (Exception)
            {
                throw;
            }

            return product;
        }

        public async Task<bool> UpdateProductByIdAsync(UpdateProductDto dto)
        {
            var sql = ProductQuery.updateProduct;
            var StockSql = ProductQuery.updateProductStock;
            bool isRowsAffected = false;

            var parameters = new
            {
                @productId = dto.ProductId,
                @productName = dto.ProductName,
                @brandId = dto.BrandId,
                @categoryId = dto.CategoryId,
                @modelyear = dto.ModelYear,
                @listPrice = dto.ListPrice,
                @productImage = dto.ImageUrl,
            };


            using (var connection = new SqlConnection(_connection.ConnectionString))
            {

                var transaction = await connection.BeginTransactionAsync();
                try
                {
                    int x = await connection.ExecuteAsync(sql, parameters, transaction: transaction);
                    //add stock for newly added product

                    var Stockparameters = new
                    {
                        @productId = dto.ProductId,
                        @stockQty = dto.StockQty
                    };

                    await connection.ExecuteAsync(StockSql, Stockparameters, transaction: transaction);

                    await transaction.CommitAsync();
                    isRowsAffected = true;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return isRowsAffected;

        }

        public async Task<bool> DeactivateProductAsync(int id)
        {

            var sql = ProductQuery.GetProduct;
            var deactivateSql = ProductQuery.deactivateProduct;

            bool isRowsAffected = false;

            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    var i = await connection.QueryAsync<ProductsDto>(sql, new { @id = id });

                    if (i.Count() > 0)
                    {
                        await connection.ExecuteAsync( deactivateSql, new { @id = id });
                        isRowsAffected = true;
                    }
                    
                }
            }
            catch (Exception)
            {
                throw;
            }

            return isRowsAffected;
        }


        public async Task CreateCategoryAsync(AddCategoryDto dto)
        {
            var query = ProductQuery.CreateCategory;

            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    await connection.ExecuteAsync(query, new { category_name = dto.CategoryName });
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<IEnumerable<CategoryDto>> GetAllCategoryAsync(string? CategoryNameFilter)
        {

            try
            {
                var query = ProductQuery.GetAllCategory;
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    var result = await connection.QueryAsync<CategoryDto>(query, new
                    {
                        @search = string.IsNullOrEmpty(CategoryNameFilter) ? null : $"%{CategoryNameFilter}%"
                    });
                    return result.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int Id)
        {
            try
            {

                var query = ProductQuery.GetCategoryById;

                //GetCategoryResponse FinalResult;
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    var FinalResult = await _connection.QueryFirstOrDefaultAsync<CategoryDto>(query, new { @CategoryId = Id });
                    return FinalResult;
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task UpdateCategoryByIdAsync(UpdateCategoryDto dto)
        {
            try
            {
                var query = ProductQuery.UpdateCategorey;

                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    await _connection.ExecuteAsync(query, new { @id = dto.CategoryId, @CategoryName = dto.CategoryName });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteCategoryByIdAsync(int Id)
        {
            try
            {
                var query = ProductQuery.DeleteCategory;

                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    await _connection.ExecuteAsync(query, new { @id = Id });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CreateBrandAsync(AddBrandDto dto)
        {
            try
            {
                var query = ProductQuery.CreateBrand;

                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    await _connection.ExecuteAsync(query, new { @BrandName = dto.BrandName, @Image = dto.ImageUrl });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<BrandsDto>> GetAllBrandsAsync(string? BrandNameFilter)
        {
            try
            {
                var query = ProductQuery.GetAllBrand;
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    var result = await _connection.QueryAsync<BrandsDto>(query, new
                    {

                        @search = string.IsNullOrEmpty(BrandNameFilter) ? null : $"%{BrandNameFilter}%"

                    });

                    return result;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BrandsDto> GetBrandByIdAsync(int Id)
        {
            try
            {
                var query = ProductQuery.GetBrandById;

                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    var FinalResult = await _connection.QueryFirstOrDefaultAsync<BrandsDto>(query, new { @id = Id });

                    return FinalResult;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteBrandByIdAsync(int Id)
        {
            try
            {
                var query = ProductQuery.DeleteById;

                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    await _connection.ExecuteAsync(query, new { @Id = Id });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<DropdownDto>> DropDownCatgoryAsync()
        {
            try
            {
                var query = ProductQuery.CategoryDropDown;

                using (var Connection = new SqlConnection(_connection.ConnectionString))
                {
                    var FinalResult = await _connection.QueryAsync<DropdownDto>(query);

                    return FinalResult.ToList();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<DropdownDto>> DropDownBrandAsync()
        {
            try
            {
                var query = ProductQuery.BrandDropDown;

                using (var connection = new SqlConnection(_connection.ConnectionString))
                {

                    var ResultSet = await _connection.QueryAsync<DropdownDto>(query);

                    return ResultSet;

                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductDropdownDto>> DropDownProductAndStockAsync(int brandId, int categoryId)
        {
            try
            {
                var query = ProductQuery.ProductDropDown;
                using(var connection = new SqlConnection(_connection.ConnectionString))
                {
                    var results = await _connection.QueryAsync<ProductDropdownDto>(query,new {
                         brandId,
                         categoryId
                    });
                    return results;
                }                                           
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}


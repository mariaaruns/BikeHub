using BikeHub.DapperQuery;
using BikeHub.Repository.IRepository;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using Dapper;
using Microsoft.Data.SqlClient;
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


                using (var connection = new SqlConnection(_connection.ConnectionString)) {

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
            var product= new GetProductByIdDto();

            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    product = await connection.QueryFirstOrDefaultAsync<GetProductByIdDto>(
                    sql,
                    new { @id=id });
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
            bool isRowsAffected= false;

            var parameters = new
            {
                @productId=dto.ProductId,
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
            bool isRowsAffected = false;

            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    int i = await connection.ExecuteAsync(sql,new { @id = id });
                    isRowsAffected = i > 0 ? true : false;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return isRowsAffected;
        }
    }
}

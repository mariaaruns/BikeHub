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
    }
}

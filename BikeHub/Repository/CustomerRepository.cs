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
    public class CustomerRepository:ICustomerRepository
    {
        private readonly IDbConnection _dbConnection;

        public CustomerRepository(IDbConnection dbConnection)
        {
            this._dbConnection = dbConnection;
        }


        public async Task<PagedResult<CustomersDto>> GetAllCustomerasync(CustomerRequest dto)
        {
            try
            {
                var query = CustomerQuery.GetAllCustomer;

                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    var totalCustomersCount=await _dbConnection.ExecuteScalarAsync<int>(CustomerQuery.TotalCustomerCount,new { 
                        @CustomerName=dto.CustomerName
                    });

                    var result = await _dbConnection.QueryAsync<CustomersDto>(query,new { 
                    @CustomerName= string.IsNullOrEmpty(dto.CustomerName) ?null : $"%{dto.CustomerName}%",
                    @offset=((dto.PageNumber - 1) * dto.PageSize),
                    @pageSize=dto.PageSize
                    });

                    return new PagedResult<CustomersDto>(totalCustomersCount, dto.PageNumber, dto.PageSize, result.ToList());
                     
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<int> AddCustomerAsync(AddCustomerDto dto)
        {
            try
            {
                var query = CustomerQuery.AddCustomer;
                var lastInsertedId = 0;
                var parameters = new
                {
                    firstName = dto.FirstName,
                    LastName = dto.LastName,
                    phone = dto.Phone,
                    email = dto.Email,
                    street = dto.Street,
                    city = dto.City,
                    state = dto.State,
                    zipCode = dto.ZipCode,
                    Image=dto.ImageUrl
                };

                using (var Connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    lastInsertedId = await _dbConnection.ExecuteScalarAsync<int>(query, parameters);
                }
                
                return lastInsertedId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateCustomerAsync(UpdateCustomerDto dto)
        {
            var query = CustomerQuery.UpdateCustomer;
            
            try
            {
                var parameters = new
                {
                    @CustomerId = dto.Id,
                    @FirstName = dto.FirstName,
                    @LastName = dto.LastName,
                    @PhoneNo = dto.Phone,
                    @MailId = dto.Email,
                    @Street = dto.Street,
                    @City = dto.City,
                    @State = dto.State,
                    @ZipCode = dto.ZipCode,
                    @Image = dto.ImageUrl
                };

                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                     await _dbConnection.ExecuteAsync(query, parameters);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task DeActivateCustomerAsync(int Id)
        {
            var query = CustomerQuery.DesActiveCustomer;

            try
            {
                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    await _dbConnection.ExecuteAsync(query, new{@Id = Id});
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CustomerDetailDto> GetCustomerByIdAsync(int Id)
        {
            var query = CustomerQuery.GetCustomerById;
            try
            {
                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    
                    var result = await connection.QueryFirstOrDefaultAsync<CustomerDetailDto>(query, new { @Id = Id });
                
                    return result;
            
                }
            
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<IEnumerable<DropdownDto>> GetCustomerDropdownAsync(string? search)
        {
            try
            {
                var query = CustomerQuery.GetCustomerDropdown;

                using (var connection = new SqlConnection(_dbConnection.ConnectionString))
                {
                    var result = await _dbConnection.QueryAsync<DropdownDto>(query, 

                        new { @search = string.IsNullOrEmpty(search) ? null : $"%{search}%"}

                    );

                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

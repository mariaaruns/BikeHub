using BikeHub.DapperQuery;
using BikeHub.Repository.IRepository;
using BikeHub.Service;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Request.AuthDto;
using BikeHub.Shared.Dto.Response;
using BikeHub.Shared.Dto.Response.Auth;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BikeHub.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;
        public UserRepository(IDbConnection connection)
        {
            this._connection = connection;
        }

        public async Task<bool> AddNewPolicy(AddPolicyDto dto)
        {
            var sql = UserAuthQuery.AddOrRemovePolicyForUser;
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {

                    var isRowsAffected = await connection.ExecuteAsync(sql,
                    new { @Code = dto.Code, @Description= dto.Description});

                    return isRowsAffected > 0;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> AddOrRemovePolicyForUser(long userId, ApplyPolicyDto[] dto)
        {
            var sql= UserAuthQuery.AddOrRemovePolicyForUser;
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    foreach(var policy in dto)
                    {
                        var isRowsAffected = await connection.ExecuteAsync(sql,
                        new { @UserId = userId, @PolicyId = policy.PolicyId, @IsActive = policy.HasPermission});

                    }

                   
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> CreateUser(CreateUserDto dto)
        {
         
            var sql = UserAuthQuery.CreateUser;
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {

                    var isRowsAffected = await connection.ExecuteAsync(sql,new { 
                    FirstName=dto.FirstName,
                    LastName=dto.LastName,
                    UserName=dto.UserName,
                    PasswordHash = dto.Password,
                    Role=dto.RoleId,
                    PhoneNumber = dto.PhoneNumber,
                    Image = dto.Image,
                    });
                    
                    
                    return isRowsAffected > 0;


                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> DeletePolicy(int policyId)
        {
            var sql = UserAuthQuery.DeletePolicy;
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    var isRowsAffected = await connection.ExecuteAsync(sql,
                    new { @PolicyId = policyId });

                    return isRowsAffected > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<PagedResult<UsersDto>> GetAllUser(UsersRequestDto dto)
        {
            try
            {
                var sql = UserAuthQuery.GetAllUsers;
                var TotalcountSql = UserAuthQuery.TotalUsersCount;

                int TotalRecordscount = 0;
                var userList = new List<UsersDto>();

                var parameters = new
                {
                    Search = string.IsNullOrEmpty(dto.SearchName) ? null : $"%{dto.SearchName}%",
                    @Role=dto.SearchRole,
                    Offset = (dto.PageNumber - 1) * dto.PageSize,
                    PageSize = dto.PageSize
                };

                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    userList = connection.Query<UsersDto>(sql, parameters).ToList();
                    TotalRecordscount = connection.ExecuteScalar<int>(TotalcountSql, parameters);
                }


                var PagedResult = new PagedResult<UsersDto>(TotalRecordscount,
                    dto.PageNumber,
                    dto.PageSize,
                    userList);

                return Task.FromResult(PagedResult);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RolesDto[]> GetRolesAsync()
        {
            var sql = UserAuthQuery.GetAllRoles;

            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {

                    var userDetails = await connection.QueryAsync<RolesDto>(sql);

                    return userDetails.ToArray();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<UserByIdDto> GetUserById(long userId)
        {
            var sql = UserAuthQuery.GetUserById;

            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {

                    var userDetails = await connection.QueryFirstOrDefaultAsync<UserByIdDto>(sql, new { @UserId = userId});

                    return userDetails;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<UsersDto?> GetUserByUserName(string userName)
        {
            var sql = UserAuthQuery.GetUserByUserName;

            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {

                    var userDetails = await connection.QueryFirstOrDefaultAsync<UsersDto>(sql,new {@UserName=userName});

                    return userDetails;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public  async Task<UsersDto> GetUserInfo(LoginDto dto)
        {
            var sql = UserAuthQuery.GettingUserInfo;
            
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    
                    var userDetails = await connection.QueryFirstOrDefaultAsync<UsersDto>(sql,
                    new { @Email = dto.Email});

                    return userDetails;
                }
            }
            catch (Exception)
            {
                throw;
            }

           

        }

        public async Task<List<UserPolicyResponse>> GetUserPoliciesByUserId(long userId)
        {
            var sql = UserAuthQuery.GetPoliciesByUserId;

            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {

                    var userDetails = await connection.QueryAsync<UserPolicyResponse>(sql,
                    new
                    {
                        UserId = userId
                    });

                    return userDetails.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> UpdatePolicy(UpdatePolicyDto dto)
        {
            var sql = UserAuthQuery.UpdatePolicy;
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    var isRowsAffected = await connection.ExecuteAsync(sql,
                    new { @PolicyId=dto.PolicyId,@Code = dto.Code, @Description = dto.Description });

                    return isRowsAffected > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateUser(UpdateUserDto dto)
        {
            var sql = UserAuthQuery.UpdateUser;
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {

                    var isRowsAffected = await connection.ExecuteAsync(sql, new
                    {
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        RoleId = dto.RoleId,
                        PhoneNumber = dto.PhoneNumber,
                        Image = dto.Image,
                        UserId=dto.UserId
                    });
                    return isRowsAffected > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

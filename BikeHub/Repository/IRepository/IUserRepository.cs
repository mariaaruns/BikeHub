using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Request.AuthDto;
using BikeHub.Shared.Dto.Response;
using BikeHub.Shared.Dto.Response.Auth;
using System.Diagnostics.Eventing.Reader;

namespace BikeHub.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<bool> CreateUser(CreateUserDto dto);
        Task<bool> UpdateUser(UpdateUserDto dto);
        Task<RolesDto[]> GetRolesAsync();
        Task<UsersDto?> GetUserByUserName(string userName);
        Task<UsersDto> GetUserInfo(LoginDto dto);
        Task<List<UserPolicyResponse>> GetUserPoliciesByUserId(long userId);
        Task<bool> AddOrRemovePolicyForUser(long userId, ApplyPolicyDto[] dto);
        Task<bool> AddNewPolicy(AddPolicyDto dto);
        Task<bool> UpdatePolicy(UpdatePolicyDto dto);
        Task<bool> DeletePolicy(int policyId);
        Task<PagedResult<UsersDto>> GetAllUser(UsersRequestDto dto);
        Task<UserByIdDto> GetUserById(long userId);

    }
}

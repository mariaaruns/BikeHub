using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Request.AuthDto;
using BikeHub.Shared.Dto.Response;
using BikeHub.Shared.Dto.Response.Auth;

namespace BikeHub.Service.Interface
{
    public interface IAuthService
    {
        //getAll policies by userId and cache it for 2 hours, if the user has no policies, cache an empty set to prevent cache penetration
        Task<HashSet<string>> GetPoliciesAsync(long userId);

        //generate JWT token and cache the policies for 2 hours
        Task<(IdentityErrorResult,JwtResponse)> GenerateJwtTokenAndPolicyAsync(LoginDto dto);


       Task<List<UserPolicyResponse>> GetAllPoliciesByUserId(long userId);

        //add or edit policy for user, if policyId is null, add new policy, otherwise edit existing policy
        Task<bool> AddOrEditPolicyForUser(long userId, ApplyPolicyDto[] dto);

        //add new policy
        Task<dynamic> AddNewPolicy();

        //remove policy by policyId
        Task<dynamic> RemovePolicy(long policyId);

        //get all menu by userId
        Task<dynamic> GetUserMenuByUserId(long userId);

        //add or edit menu, if menuId is null, add new menu, otherwise edit existing menu
        Task<dynamic> AddOrEditMenu(long? menuId);

        //remove menu by menuId
        Task<dynamic> RemoveMenu();

        //get all users with role name
        Task<PagedResult<UsersDto>> GetAllUser(UsersRequestDto dto);

        //get User by userId with role name
        

        //create new user only by admin policy
        Task<bool> CreateUser(CreateUserDto dto);
        
        //update user, if password is null, keep the original password, if roleId is null, keep the original roleId, if firstName is null, keep the original firstName, if lastName is null, keep the original lastName, if phoneNumber is null, keep the original phoneNumber
        Task<bool> UpdateUser(UpdateUserDto dto);


        Task<UsersDto> GetUserByUserName(string userName);

        Task<RolesDto[]> GetAllRoles();

        Task<UserByIdDto> GetUserById(long userId);
    }
}

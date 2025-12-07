using BikeHub.Models;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using Microsoft.AspNetCore.Identity;

namespace BikeHub.Service.Interface
{
    public interface IApplicationUserStore<T>: IUserStore<T> where T : ApplicationUser
    {
        Task<PagedResult<UsersDto>> GetAllUsersAsync(UsersRequestDto dto, CancellationToken cancellationToken);

    }

   
}

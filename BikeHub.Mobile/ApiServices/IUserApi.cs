using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using res=BikeHub.Shared.Common;
namespace BikeHub.Mobile.ApiServices
{
    public interface IUserApi
    {
        [Post("/createUser")]
        Task<res.ApiResponse<string>> CreateUser([Body] RegisterDto dto);

        [Post("/login")]
        Task<res.ApiResponse<JwtResponse>> Login([Body] LoginDto dto);

        [Get("/users")]
        Task<res.ApiResponse<res.PagedResult<UsersDto>>> GetUsersAsync([Query] UsersRequestDto dto, CancellationToken cancellationToken);



    }
}

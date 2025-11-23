using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using res= BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Response;
using Refit;
namespace BikeHub.Mobile.ApiServices
{
    public interface IBrandApi
    {
        [Get("GetAllBrand")]
        Task<res.ApiResponse<IEnumerable<BrandsDto>>> GetAllBrandsAsync();

    }
}

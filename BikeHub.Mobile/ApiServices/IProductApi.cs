
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using res = BikeHub.Shared.Common;
namespace BikeHub.Mobile.ApiServices
{
   public interface IProductApi
    {

        [Post("/products")]
        Task<res.ApiResponse<res.PagedResult<ProductsDto>>> GetProducts(GetProductsDto dto, CancellationToken cancellationToken);
    }
}

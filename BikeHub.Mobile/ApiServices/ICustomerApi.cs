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
    public interface ICustomerApi
    {

        [Post("/GetAllCustomers")]
        Task<res.ApiResponse<res.PagedResult<CustomersDto>>> GetCustomersAsync(CustomerRequest dto, CancellationToken cancellationToken);
        [Get("/GetCustomerById")]
        Task<res.ApiResponse<CustomerDetailDto>> GetCustomerByIdAsync(int Id, CancellationToken cancellationToken);
        [Post("/AddCustomer")]
        Task<res.ApiResponse<int>> AddCustomerAsync(MultipartFormDataContent dto, CancellationToken cancellationToken);
        [Put("/UpdateCustomer")]
        Task<res.ApiResponse<bool>> UpdateCustomerAsync(MultipartFormDataContent dto, CancellationToken cancellationToken);
        
        [Delete("/DeActivateCustomer/{id}")]
        Task<res.ApiResponse<string>> DeActivateCustomerAsync(int id, CancellationToken cancellationToken);

        [Get("/Dropdown")]
        Task<res.ApiResponse<IEnumerable<DropdownDto>>> GetCustomerDrpodown(CancellationToken cancellationToken,string type="customer",string? search=null);
    }
}

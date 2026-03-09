using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;

namespace BikeHub.Repository.IRepository
{
    public interface ICustomerRepository
    {
        Task<PagedResult<CustomersDto>> GetAllCustomerasync(CustomerRequest dto);

        Task<CustomerDetailDto> GetCustomerByIdAsync(int Id);

        Task<int> AddCustomerAsync(AddCustomerDto dto);

        Task<bool> UpdateCustomerAsync(UpdateCustomerDto dto);

        Task DeActivateCustomerAsync(int id);

        Task<IEnumerable<DropdownDto>> GetCustomerDropdownAsync(string? search);
    }
}

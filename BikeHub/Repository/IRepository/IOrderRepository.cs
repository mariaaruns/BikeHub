using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;

namespace BikeHub.Repository.IRepository
{
    public interface IOrderRepository
    {
        Task<PagedResult<OrdersDto>> GetOrdersAsync(GetOrderDto dto);
        
        Task<IEnumerable<OrderDetailsDto>> GetOrderDetailsAsync(int OrderId);
        Task<bool> AddOrderAsync(AddOrderRequest dto);

        Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto dto);

        Task<IEnumerable<DropdownDto>> GetOrderStatusDropdownAsync();
    }
}

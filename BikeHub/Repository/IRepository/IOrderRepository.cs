using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using static BikeHub.Shared.Enum.Enums;

namespace BikeHub.Repository.IRepository
{
    public interface IOrderRepository
    {
        Task<PagedResult<OrdersDto>> GetOrdersAsync(GetOrderDto dto);
        Task<OrderDetailsDto> GetOrderDetailsAsync(int OrderId);
        Task<long> AddOrderAsync(AddOrderRequest dto);
        Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto dto);
        Task<IEnumerable<DropdownDto>> GetOrderStatusDropdownAsync();
        Task<bool> UpdateOrderPaymentStatusAsync(long orderId, string RazorpayOrderId,PaymentStatus paymentStatus);
        Task<bool> ConfirmPaymentAndQueueEmailAsync(int orderId, string paymentId);
    }
}

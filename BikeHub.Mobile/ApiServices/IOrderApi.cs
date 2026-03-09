using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using Refit;
using res = BikeHub.Shared.Common;
namespace BikeHub.Mobile.ApiServices
{
    public interface IOrderApi
    {
        //order consumes here
        [Post("/orders")]
        Task<res.ApiResponse<res.PagedResult<OrdersDto>>> GetOrdersAsync(GetOrderDto dto, CancellationToken cancellationToken);
       
        [Post("/addOrders")]
        Task<res.ApiResponse<string>> AddOrderAsync(AddOrderRequest dto, CancellationToken cancellationToken);

        [Post("/updateOrderStatus")]
        Task<res.ApiResponse<string>> UpdateOrderStatusAsync(UpdateOrderStatusDto dto, CancellationToken cancellationToken);

        [Get("/orderDetailWithItems")]
        Task<res.ApiResponse<OrderDetailsDto>> GetOrderDetailWithItemsAsync(int orderId, CancellationToken cancellationToken);
        
        [Get("/Dropdown")]
        Task<res.ApiResponse<IEnumerable<DropdownDto>>> GetOrderStatusAsync( CancellationToken cancellationToken, string type = "orderstatus");

    }
}

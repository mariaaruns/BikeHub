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
    public interface IDashBoardApi
    {
        [Get("/getcount")]
        Task<res.ApiResponse<DashboardResponseDto>> DashboardCount();

        [Get("/dashboardSalesAmount")]
        Task<res.ApiResponse<IEnumerable<SalesAmountByYearDto>>> DashboardSalesAmount(int year);
    }
}


using BikeHub.Mobile.ApiServices;
using BikeHub.Mobile.Pages;
using BikeHub.Shared.Dto.Response;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.Maui.Toolkit.Picker;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Mobile.ViewModel
{
    
    public partial class OrderViewModel : ObservableObject
    {
        [ObservableProperty]
        private string orderId;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private bool _isLoadingMore;

        public string PageTitle => string.IsNullOrEmpty(OrderId)
                                      ? "Add Order"
                                      : "Edit Order - #" + OrderId;
     
        [ObservableProperty]
        private ObservableCollection<OrdersDto> _orders = new();

        [ObservableProperty]
        private DropdownDto selectedStatus;

        [ObservableProperty]
        private DateTime _selectedOrderDate =DateTime.Now;
        
        [ObservableProperty]
        private int? _searchOrderId;

        private CancellationTokenSource _searchOrderIdCts;
        async partial void OnSearchOrderIdChanged(int? value)
        {
            try
            {
                _searchOrderIdCts?.Cancel();

                _searchOrderIdCts = new CancellationTokenSource();

                var token = _searchOrderIdCts.Token;

                await Task.Delay(500, token);
                Orders.Clear();
                _ = LoadOrdersAsync(token);

            }
            catch (TaskCanceledException)
            {
                //ignore
            }
            catch (Exception)
            {

                
            }


        }

        private CancellationTokenSource _selectedOrderDateCts;

        async partial void OnSelectedOrderDateChanged(DateTime value)
        {
            try
            {
                _selectedOrderDateCts?.Cancel();
                
                _selectedOrderDateCts = new CancellationTokenSource();
         
                var token = _selectedOrderDateCts.Token;

                await Task.Delay(500, token);
                Orders.Clear();
                _currentPage = 1;
                _ = LoadOrdersAsync(token);

            }
            catch (TaskCanceledException) 
            { 
            //ignore
            }
            catch (Exception)
            {
            }
                
            
        }

        [ObservableProperty]
        private List<DropdownDto> _statusSource=new();


        private readonly IOrderApi _orderApi;
        public OrderViewModel(IOrderApi orderApi)
        {
            _orderApi = orderApi;
        }

        [RelayCommand]
        private async Task StatusChangedAsync(object selectedItem)
        {
            if (selectedItem != null && selectedItem is DropdownDto obj)
            {
                Orders.Clear();
                SelectedStatus = obj;
                _currentPage = 1;
                await LoadOrdersCommand.ExecuteAsync(null);
            }

        }



        [RelayCommand]
        public async Task GotoNewOrder()
        {

            await Shell.Current.GoToAsync($"{nameof(AddEditOrders)}");

        }


        [RelayCommand]
        public async Task CreateNewOrderAsync()
        {
            await Shell.Current.GoToAsync($"{nameof(OrderPlacedPage)}");
        }

        [RelayCommand]
        private async Task LoadOrderstaus() 
        {
            try
            {
                var result= await _orderApi.GetOrderStatusAsync(CancellationToken.None);

                StatusSource   = result?.Data?.ToList();
                SelectedStatus = StatusSource?.FirstOrDefault();
            }
            catch (Exception)
            {

                
            }
        }




        private int _currentPage = 1;
        private int _pageSize = 20;
        [RelayCommand]
        private async Task LoadOrdersAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (IsLoadingMore) return;

                IsLoadingMore = true;

                var dto = new BikeHub.Shared.Dto.Request.GetOrderDto
                {
                    StartDate = SelectedOrderDate,
                    OrderStatus = SelectedStatus.Value,
                    PageSize = _pageSize,
                    PageNumber = _currentPage,
                    OrderId = SearchOrderId
                };

                var orders = await _orderApi.GetOrdersAsync(dto, cancellationToken);

                if (orders?.Data == null || !orders.Data.Data.Any())
                {
                    IsLoadingMore = false;
                    return;

                }

                foreach (var order in orders.Data.Data)
                {
                    Orders.Add(order);
                }

                _currentPage++;

            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log error, show message to user)
            }
            finally 
            { 
                IsLoadingMore = false;
                IsRefreshing=false;
            }

        }

        [RelayCommand]
        private async Task RefreshOrdersAsync() 
        {
            try
            {
                IsRefreshing = true;
                Orders.Clear();
                _currentPage = 1;
                await LoadOrdersAsync(CancellationToken.None);
            }
            catch (Exception)
            {

            }
            finally { IsRefreshing = false; }
        }
    }
}

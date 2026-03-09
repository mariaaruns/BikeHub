using BikeHub.Mobile.ApiServices;
using BikeHub.Mobile.Pages;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Mobile.ViewModel
{

    [QueryProperty(nameof(CustomerId), "customerId")]
    public partial class CustomerViewModel : ObservableObject
    {
        private readonly ICustomerApi  _customerApi;
        public CustomerViewModel(ICustomerApi api)
        {
            _customerApi=api;
        }

        [ObservableProperty]
        private string customerId;

        [ObservableProperty]
        private ObservableCollection<CustomersDto> _customers = new();

        [ObservableProperty]
        private string _customerName;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private bool _isLoadingMore;

        public string PageTitle => string.IsNullOrEmpty(customerId) ? "Add Customer" : "Edit Customer";

        partial void OnCustomerIdChanged(string value)
        {
            OnPropertyChanged(nameof(PageTitle));
        }


        private CancellationTokenSource _searchCts;

        async partial void OnCustomerNameChanged(string newValue)
        {
            try
            {
                _searchCts?.Cancel();
                _searchCts = new CancellationTokenSource();
                var token = _searchCts.Token;
                await Task.Delay(500, token);
                Customers.Clear();
                _currentPage = 1;
                _ = LoadCustomersAsync(token);

            }
            catch (TaskCanceledException)
            {

            }
            catch (Exception)
            {

                throw;
            }


        }

        private int _currentPage = 1;
        private int _pageSize = 20;
        
        [RelayCommand]
        public async Task  LoadCustomersAsync(CancellationToken token)
        {
            try
            {
                if (IsLoadingMore) return;

                IsLoadingMore = true;
                
                var req = new CustomerRequest
                {
                    PageSize = _pageSize,
                    PageNumber = _currentPage,
                    CustomerName = CustomerName
                };

                var result =  await _customerApi.GetCustomersAsync(req,  token);

                if (result is null)
                {
                    IsLoadingMore = false;
                    return;
                }
            
                if (result != null && result.Status && result.Data != null)
                {
                    foreach (var customer in result.Data.Data)
                    {
                        Customers.Add(customer);
                    }

                    _currentPage++;
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally 
            {
                IsLoadingMore = false;
                IsRefreshing=false;
            }

        }

        [RelayCommand]
        private async Task RefreshAsync()
        {

            IsRefreshing = true;
            try
            {
                _currentPage = 1; // Reset to the first page
                var cts = new CancellationTokenSource();
                Customers.Clear();
                await LoadCustomersAsync(cts.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Refresh Failed: {ex.Message}");
            }
            finally
            {

                IsRefreshing = false;
            }
        }

        [RelayCommand]
        public async Task GotoAddCustomer()
        {
            await Shell.Current.GoToAsync($"{nameof(AddEditCustomer)}");
        }

        [RelayCommand]
        async Task GoToDetails(CustomersDto dto)
        {
            if (dto is null) return;

            await Shell.Current.GoToAsync(nameof(CustomerDetailsPage)+$"?customerId={dto.CustomerId}");
        }
        [RelayCommand]
        private async Task DeleteCustomerAsync(CustomersDto dto) { 
        
            if (dto is null) return;

            try
            {
                var confirm = await Application.Current.MainPage.DisplayAlert("Confirm Delete", $"Are you sure you want to delete {dto.CustomerName}?", "Yes", "No");

                if(!confirm) return;

                var result = await _customerApi.DeActivateCustomerAsync(dto.CustomerId,CancellationToken.None);

                if (result != null && result.Status)
                {
                    Customers.Remove(dto);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error","Internal Server Error","Ok");
            }

        }
    }
}

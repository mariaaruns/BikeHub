using BikeHub.Mobile.ApiServices;
using BikeHub.Mobile.Models;
using BikeHub.Mobile.Pages;
using BikeHub.Shared.Dto.Response;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace BikeHub.Mobile.ViewModel
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly IDashBoardApi _dashboardApi;

        [ObservableProperty]
        public ObservableCollection<SalesAmountByYearDto> _data;
      
        [ObservableProperty]
        private int _totalProductsCount;

        [ObservableProperty]
        private int _totalOrdersCount;
        
        [ObservableProperty]
        private int _totalServiceCount;
        
        
        [ObservableProperty]
        private int _pendingServiceCount;
        
        [ObservableProperty]
        private int _completedServiceCount;

        [ObservableProperty]
        private decimal _currentMonthSalesAmount;


        [ObservableProperty]
        private DateTime _dateSelected= DateTime.Now;
        [ObservableProperty]
        private bool _isBusy;
        [ObservableProperty]
        private string? _errorMessage;

        [ObservableProperty]
        private bool _isCountsLoading;

        [ObservableProperty]
        private bool _isDashboardLoading;

        public string DateSelectedText => DateSelected.ToString("MM/yyyy");
        public DashboardViewModel(IDashBoardApi dashBoardApi)
        {
            _dashboardApi = dashBoardApi;
            //Data = new ObservableCollection<SalesChartModel>
            //{
            //    new SalesChartModel("Jan", 3500),
            //    new SalesChartModel("Feb", 280),
            //    new SalesChartModel("Mar", 3400),
            //    new SalesChartModel("Apr", 3200),
            //    new SalesChartModel("Jun", 4000),
            //    new SalesChartModel("Jul", 5000),
            //    new SalesChartModel("Aug", 6000),
            //    new SalesChartModel("Sep", 7000),
            //    new SalesChartModel("Oct", 8000),
            //    new SalesChartModel("Nov", 12000),
            //    new SalesChartModel("Dec", 15000)
                
            //};
        }

        partial void OnDateSelectedChanged(DateTime value)
        {
            // Notify that the formatted text changed
            OnPropertyChanged(nameof(DateSelectedText));

            // Optional: refresh dashboard counts for the selected month/year.
            // If your API accepts a date filter, call it here (adapt LoadDashboardAsync signature if needed).
            // Fire-and-forget is used here to avoid blocking the UI; handle errors/logging as required.
            _ = LoadDashboardAsync(CancellationToken.None);
            //_ = LoadSalesChartCommand(CancellationToken.None);
        }


        [RelayCommand]
        public async Task GotoCreateOrderPageAsync()
            => await Shell.Current.GoToAsync(nameof(AddEditOrders));

        [RelayCommand]
        public async Task LoadDashboardAsync(CancellationToken cancellationToken)
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                ErrorMessage = null;
                IsCountsLoading = true;
                // Respect cancellation
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay(1000, cancellationToken); 
                var result = await _dashboardApi.DashboardCount();

                // simple null/validity checks
                if (result is null)
                {
                    ErrorMessage = "No response from server.";
                    //Debug.WriteLine("DashboardCount returned null.");
                    return;
                }

                if (!result.Status)
                {
                    ErrorMessage = string.IsNullOrWhiteSpace(result.Message) ? "Failed to load dashboard data." : result.Message;
                    //Debug.WriteLine($"DashboardCount failed: {result.Message}");
                    return;
                }

                if (result.Data is null)
                {
                    ErrorMessage = "Server returned no dashboard data.";
                  //  Debug.WriteLine("DashboardCount returned an empty Data payload.");
                    return;
                }
                

                // Update properties (keeps UI updates in the UI thread context since bindings will handle it)
                TotalProductsCount = result.Data.TotalProductsCount;
                TotalOrdersCount = result.Data.TotalOrdersCount;
                TotalServiceCount = result.Data.TotalServiceCount;
                PendingServiceCount = result.Data.PendingServiceCount;
                CompletedServiceCount = result.Data.CompletedServiceCount;
            }
            catch (OperationCanceledException)
            {
                // Cancellation is expected sometimes; don't treat as an error.
                //Debug.WriteLine("LoadDashboardAsync was cancelled.");
            }
            catch (Exception ex)
            {
                ErrorMessage = "An unexpected error occurred while loading dashboard.";
                //Debug.WriteLine($"Exception in LoadDashboardAsync: {ex}");
            }
            finally
            {
                IsCountsLoading = false;
                IsBusy = false;
            }


        }

        [RelayCommand]
        private async Task LoadSalesChart(CancellationToken cancellationToken) 
        {

            try
            {
                IsDashboardLoading = true;
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _dashboardApi.DashboardSalesAmount(2018);
            
                if (result is null)
                {
                    ErrorMessage = "No response from server.";
                    //Debug.WriteLine("DashboardCount returned null.");
                    return;
                }

                if (!result.Status)
                {
                    ErrorMessage = string.IsNullOrWhiteSpace(result.Message) ? "Failed to load dashboard data." : result.Message;
                    //Debug.WriteLine($"DashboardCount failed: {result.Message}");
                    return;
                }

                if (result.Data is null)
                {
                    ErrorMessage = "Server returned no dashboard data.";
                    //  Debug.WriteLine("DashboardCount returned an empty Data payload.");
                    return;
                }
                var s = DateTime.Now.ToString("MMM");
                var getCurrentAmount = result.Data.Where(x => x.Month == DateTime.Now.ToString("MMM")).FirstOrDefault();
                if (getCurrentAmount is null)
                    CurrentMonthSalesAmount = 0m;
                else
                    CurrentMonthSalesAmount = getCurrentAmount.NetAmount;


                
                Data = result.Data.ToObservableCollection();


            }
            catch (OperationCanceledException)
            {
                // Cancellation is expected sometimes; don't treat as an error.
                //Debug.WriteLine("LoadDashboardAsync was cancelled.");
            }
            catch (Exception)
            {
                ErrorMessage = "An unexpected error occurred while loading dashboard.";
                
            }
            finally
            {
                IsDashboardLoading = false;

            }
        
        
        }

        [RelayCommand]
        public  async Task DateChanged() {

            var date = DateSelected;
        }
    }
}

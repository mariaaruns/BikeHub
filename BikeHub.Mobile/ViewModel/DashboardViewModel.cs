using BikeHub.Mobile.ApiServices;
using BikeHub.Mobile.Models;
using BikeHub.Mobile.Pages;
using BikeHub.Shared.Dto.Response;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
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
           
        }

        partial void OnDateSelectedChanged(DateTime value)
        {
            // Notify that the formatted text changed
            OnPropertyChanged(nameof(DateSelectedText));

            // Optional: refresh dashboard counts for the selected month/year.
            // If your API accepts a date filter, call it here (adapt LoadDashboardAsync signature if needed).
            // Fire-and-forget is used here to avoid blocking the UI; handle errors/logging as required.
            _ = LoadallDataDashboard(CancellationToken.None);
            //_ = LoadSalesChartCommand(CancellationToken.None);
        }


        [RelayCommand]
        public async Task GotoCreateOrderPageAsync()
            => await Shell.Current.GoToAsync(nameof(AddEditOrders));

        
        public async Task LoadDashboardAsync(CancellationToken cancellationToken)
        {
            try
            {
                ErrorMessage = null;
                IsCountsLoading = true;
                // Respect cancellation
                cancellationToken.ThrowIfCancellationRequested();
                
                await Task.Delay(100, cancellationToken); 
                
                var result = await _dashboardApi.DashboardCount(DateSelected);

                // simple null/validity checks
                if (result is null)
                {
                    ErrorMessage = "No response from server.";
                    
                    return;
                }

                if (!result.Status)
                {
                    ErrorMessage = string.IsNullOrWhiteSpace(result.Message) ? "Failed to load dashboard data." : result.Message;
                    
                    return;
                }

                if (result.Data is null)
                {
                    ErrorMessage = "Server returned no dashboard data.";
                  
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
                
            }
            catch (Exception ex)
            {
                ErrorMessage = "An unexpected error occurred while loading dashboard.";
                
            }
            finally
            {
                IsCountsLoading = false;
                IsBusy = false;
            }


        }

        private async Task LoadSalesChart(CancellationToken cancellationToken) 
        {

            try
            {
                IsDashboardLoading = true;
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _dashboardApi.DashboardSalesAmount(DateSelected.Year);
            
                if (result is null)
                {
                    ErrorMessage = "No response from server.";
                    return;
                }

                if (!result.Status)
                {
                    ErrorMessage = string.IsNullOrWhiteSpace(result.Message) ? "Failed to load dashboard data." : result.Message;
                    return;
                }

                if (result.Data is null)
                {
                    ErrorMessage = "Server returned no dashboard data.";
                    
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
                ErrorMessage = "An unexpected error occurred while loading dashboard chart.";
                
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

        [RelayCommand]
        public async Task LoadallDataDashboard(CancellationToken cancellationToken) 
        {

            try
            {
                if (IsBusy) return;
                IsBusy = true;
                

                var task1 = LoadDashboardAsync(cancellationToken);
                var task2 = LoadSalesChart(cancellationToken);

                await Task.WhenAll(task1, task2);

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    var toast =  Toast.Make(ErrorMessage, ToastDuration.Long);
                    await toast.Show(cancellationToken);
                }
            }
            catch(Exception ex)
            {

            }

            finally {
                IsBusy = false;
            }
        }
    }
}

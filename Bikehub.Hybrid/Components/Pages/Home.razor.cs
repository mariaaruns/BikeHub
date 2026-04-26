

using Bikehub.Hybrid.Authhandler;
using Bikehub.Hybrid.DeviceServices.Location;
using Bikehub.Hybrid.DeviceServices.Toast;
using Bikehub.Hybrid.Services.Http.ServiceDashboard;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using BikeHub.Shared.Dto.Response.ServiceRes;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;



namespace Bikehub.Hybrid.Components.Pages
{
    public partial class Home : ComponentBase, IDisposable
    {
        [Inject]
        private IServiceDashboard _serviceDashboard { get; set; } = default!;
        [Inject]
        private UserSession _userSession { get; set; } = default!;

        [Inject]
        private  IToastService _toastService { get; set; } = default!;

        [Inject]
        private ILocationService _locationService { get; set; } = default!;

        [Inject]
        private  IJSRuntime JSRuntime { get; set; } = default!;

        private MechanicTaskSummayDto _summary = new();
        
        private ServiceJobDetailDto _serviceJobDetail = new();
        
        private AddServiceItemsDto _addServiceItems = new();

        private List<ServiceItemDto> _serviceItems = new();

        private IEnumerable<DropdownDto> _categoryDropdown = Enumerable.Empty<DropdownDto>();
        private IEnumerable<PartsDto> _partsDropdown = Enumerable.Empty<PartsDto>();
        List<PartsDto> FilteredParts = new();
        private AssignedJobVM _assignedJobVM = new AssignedJobVM();

        private bool _isMechSummaryLoading;

        private bool _isJobsLoading;

        private bool _isJobDetailLoading;

        private bool _isStartJobLoading;

        private bool _isCompleteJobLoading;

        bool showModal = false;
        bool showServiceItemsModal = false;

        private decimal? _partCost = 0m;

        private System.Timers.Timer? timer;

        enum TabType { Pending, Progress, Completed }

        TabType activeTab = TabType.Pending;
        protected override async Task OnInitializedAsync()
        {
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += (_, __) => InvokeAsync(StateHasChanged);
            timer.Start();

            await MechanicWorkSummaryApi();
            await AssignedJobsApi();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) { 
            
                _ = Task.Run(async () =>
                    {
                        _categoryDropdown = await DropdownLookup("servicepartscategory");
                         await PartsListApi();
                    });
            }
            return base.OnAfterRenderAsync(firstRender);
        }



        void SetTab(TabType tab)
        {
            activeTab = tab;
        }
        private async Task MechanicWorkSummaryApi()
        {
            try
            {
                _isMechSummaryLoading = true;
                int mechanicId = int.Parse(_userSession.UserId ?? "0");
                var response = await _serviceDashboard.MechanicWorkSummaryAsync(mechanicId);
                if (response.Status && response.Data != null)
                {
                    _summary = response.Data;
                }
                else if(!response.Status)
                {
                    Console.WriteLine($"Error fetching summary: {response.Message}");
                    await ShowToast("error", "Internal Server error", true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while fetching summary: {ex.Message}");
                await ShowToast("error", "Something went wrong on our end", true);
            }
            finally
            {
                _isMechSummaryLoading = false;
            }
        }
        private async Task AssignedJobsApi()
        {
            int mechanicId = int.Parse(_userSession.UserId ?? "0");
            try
            {
                _isJobsLoading = true;
                var response = await _serviceDashboard.AssignedJobsAsync(mechanicId);
                if (response.Status && response.Data != null)
                {
                    IEnumerable<AssignedJobDto> _assignedJobs
                                                  = Enumerable.Empty<AssignedJobDto>();
                    _assignedJobs = response.Data;
                    _assignedJobVM._pendingJobs =
                                    _assignedJobs
                                    .Where(j => j.ServiceStatus.Equals("pending", StringComparison.OrdinalIgnoreCase));
                    _assignedJobVM._InProgresssJobs =
                                    _assignedJobs
                                    .Where(j => j.ServiceStatus.Equals("Inprogress", StringComparison.OrdinalIgnoreCase));
                    _assignedJobVM._CompletedJobs =
                                    _assignedJobs
                                    .Where(j => j.ServiceStatus.Equals("completed", StringComparison.OrdinalIgnoreCase));


                }
                else if (!response.Status)
                {
                    Console.WriteLine($"Error fetching summary: {response.Message}");
                    await ShowToast("error", "Internal Server error", true);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while fetching summary: {ex.Message}");
                await ShowToast("error", "Something went wrong on our end", true);
            }
            finally
            {
                _isJobsLoading = false;
            }
        }

        private async Task JobDetailApi(long jobId)
        {
            try
            {
                _isJobDetailLoading = true;
                _serviceJobDetail = new ServiceJobDetailDto();
                var response = await _serviceDashboard.JobDetailsAsync(jobId);
                if (response.Status)
                {

                    _serviceJobDetail = response?.Data;

                }
                else
                {
                    await ShowToast("error", "Internal Server error", true);
                }
            }
            catch (Exception ex)
            {
                await ShowToast("error", "Something went wrong on our end", true);
            }
            finally
            {
                _isJobDetailLoading = false;
            }
        }
        private async Task StartJobApi(long jobId)
        {
            try
            {
                _isStartJobLoading = true;

                //var location=await _locationService.GetCurrentLocationAsync();

                var response = await _serviceDashboard.StartJobAsync(jobId);

                if (response.Status)
                {
                    SetTab(TabType.Progress);

                    await Task.WhenAll(AssignedJobsApi(), MechanicWorkSummaryApi());

                }
                else
                {
                    //Console.WriteLine($"Error starting job: {response.Message}");
                    await _toastService.Show("Error starting job");
                }
            }
            catch (Exception ex)
            {
                //toast error
                await _toastService.Show("Error starting job");
                Console.WriteLine($"Error fetching job details: {ex.Message}");

            }
            finally
            {
                _isStartJobLoading = false;
                StateHasChanged();
            }
        }
        private async Task CompleteJobApi(long jobId)
        {
            try
            {
                _isCompleteJobLoading = true;
                var response = await _serviceDashboard.CompleteJobAsync(jobId);
                if (response.Status)
                {
                    SetTab(TabType.Completed);
                    await Task.WhenAll(AssignedJobsApi(),MechanicWorkSummaryApi());
                }
                else
                {
                    //toast error
                    await _toastService.Show("Error completing job");
                }
            }
            catch (Exception ex)
            {
                //toast error
                await _toastService.Show("Error completing job");
                Console.WriteLine($"Error completing job: {ex.Message}");
            }
            finally 
            {
                _isCompleteJobLoading = false;
                StateHasChanged();
            }
        }


        private async Task ServiceItemsApi(long jobId)
        {
            try
            {
                var response = await _serviceDashboard.ServiceItems(jobId);
                if (response.Status)
                {
                    _serviceItems = response.Data != null ? response.Data: new();
                }
                else
                {
                    await ShowToast("error", "Failed to fetch service items", true);
                }
            }
            catch (Exception ex)
            {
                await ShowToast("error", "Something went wrong on our end", true);
                Console.WriteLine($"Error fetching service items: {ex.Message}");
            }
        }

        private async Task PartsListApi()
        {
            try
            {
                var response = await _serviceDashboard.PartsList();
                if (response.Status)
                {
                    _partsDropdown = response.Data != null ? response.Data : Enumerable.Empty<PartsDto>();
                }
                else
                {
                    await ShowToast("error", "Failed to fetch parts list", true);
                }
            }
            catch (Exception ex)
            {
                await ShowToast("error", "Something went wrong on our end", true);
                Console.WriteLine($"Error fetching parts list: {ex.Message}");
            }
        }

        private async Task<IEnumerable<DropdownDto>> DropdownLookup (string value)
        {
            var dropdowns  = Enumerable.Empty<DropdownDto>();
             
            try
            {
                var response = await _serviceDashboard.DropdownLookup(value);
                if (response.Status)
                {
                    dropdowns = response.Data;
                }
                else
                {
                    await ShowToast("error", "Failed to fetch dropdown data", true);
                }
            }
            catch (Exception ex)
            {
                await ShowToast("error", "Something went wrong on our end", true);
                Console.WriteLine($"Error fetching dropdown data: {ex.Message}");
            }
            return dropdowns;
        }

        private async Task AddServiceItemsApi()
        {
            try
            {
                var response = await _serviceDashboard.AddServiceItems(_addServiceItems);
                if (response.Status)
                {
                    await ShowToast("success", "Service items added successfully", true);

                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                //await ShowToast("error", "Something went wrong on our end", true);
                Console.WriteLine($"Error adding service items: {ex.Message}");
                throw;
            }
            finally {
                
            }
        }
        public void Dispose()
        {
            timer?.Stop();
            timer?.Dispose();
            timer = null;
        }
        public class AssignedJobVM
        {
            public IEnumerable<AssignedJobDto>? _pendingJobs { get; set; }
            public IEnumerable<AssignedJobDto>? _InProgresssJobs { get; set; }
            public IEnumerable<AssignedJobDto>? _CompletedJobs { get; set; }

            public int _pendingCount => _pendingJobs?.Count() ?? 0;
            public int _inProgressCount => _InProgresssJobs?.Count() ?? 0;
            public int _completedCount => _CompletedJobs?.Count() ?? 0;
        }
        private string GetElapsed(DateTime startTime)
        {
            var elapsed = DateTime.Now - startTime;

            return $"{(int)elapsed.TotalHours:00}:{elapsed.Minutes:00}:{elapsed.Seconds:00}";
        }
        private void CloseModal()
        {
            _serviceJobDetail = new ServiceJobDetailDto();
            showModal = false;
        }

        private async Task OpenModal(long jobId)
        {
           
            showModal = true;
            await JobDetailApi(jobId);
        }
        private string IfEmptyShowNA(object value)
        {
            if (value == null)
                return "N/A";

            if (value is string str && string.IsNullOrWhiteSpace(str))
                return "N/A";

            if (value is DateTime dt && dt == default)
                return "N/A";

            if (value is decimal dec && dec == 0)
                return "N/A";

            return value.ToString();
        }


        private async Task OpenServiceItemsModal(long jobId)
        {
            _addServiceItems.ServiceJobId = 0;
            showServiceItemsModal = true;
            _addServiceItems.ServiceJobId = jobId;
            await ServiceItemsApi(jobId);
        }
      
        private async Task CloseServiceItemsModal() {

            _addServiceItems = new AddServiceItemsDto();
          showServiceItemsModal = false;
        }

        private async Task HandleSubmit()
        {
            try
            {
                var selectedPart = _partsDropdown
                    .FirstOrDefault(p => p.PartId == _addServiceItems.PartId);

                if (selectedPart == null)
                    return;

                await AddServiceItemsApi();
                // Reset form if serivice added;
                _addServiceItems.Total = 0m;
                _addServiceItems.Qty = 0;
                _addServiceItems.PartId = 0;
                _addServiceItems.CreatedAt = default;
                _partCost = 0;

                await ServiceItemsApi(_addServiceItems.ServiceJobId);
                
            }
            catch (Exception ) 
            {
                await ShowToast("error", "Failed to add service items", true);
            }
        }
        private void OnCategoryChanged(ChangeEventArgs e)
        {
            int categoryId = Convert.ToInt32(e.Value);
            _addServiceItems.PartId = 0;
            _addServiceItems.Qty = 0;
            _addServiceItems.Total = 0;
           FilteredParts = _partsDropdown
                            .Where(p => p.CategoryId == categoryId)
                            .ToList();
        }

        private void OnPartChanged()
        {
            var selectedPart = _partsDropdown
                        .FirstOrDefault(p => p.PartId == _addServiceItems.PartId);

            if (selectedPart != null)
            {
                _partCost = selectedPart.Price;
                _addServiceItems.Qty = 1;
            }
            else {
                _partCost = 0;
            }
                Recalculate();
        }
        private void OnQtyChanged()
        {
            Recalculate();
        }


        private void Recalculate()
        {
            if (_partCost.HasValue)
                _addServiceItems.Total = _addServiceItems.Qty * _partCost.Value;
        }
        private async Task ShowToast(string type, string message, bool showBar)
        {
            await JSRuntime.InvokeVoidAsync("showToastrAdvanced", type, message, showBar);
        }
        
        
    }
}

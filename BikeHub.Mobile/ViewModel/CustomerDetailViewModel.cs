using Android.Database;

using BikeHub.Mobile.ApiServices;
using BikeHub.Mobile.Pages;
using BikeHub.Shared.Dto.Response;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Mobile.ViewModel
{
    [QueryProperty(nameof(CustomerId), "customerId")]
    public partial class CustomerDetailViewModel : ObservableObject
    {
        private readonly ICustomerApi _customerApi;
        public CustomerDetailViewModel(ICustomerApi customerApi)
        {
            this._customerApi = customerApi;
        }

        [ObservableProperty]
        private string _customerId;
        [ObservableProperty]
        private string _firstName ;
        [ObservableProperty]
        private string _lastName ;
        [ObservableProperty]
        private string _customerName ;
        [ObservableProperty]
        private string _email ;
        [ObservableProperty]
        private string _phone ;
        [ObservableProperty]
        private string _street ;
        [ObservableProperty]
        private string _city ;
        [ObservableProperty]
        private string _state ;
        [ObservableProperty]
        private string _zipCode ;
        [ObservableProperty]
        private string _Image ;

        [ObservableProperty]
        private bool _isBusy;

        public string PageTitle => string.IsNullOrEmpty(CustomerId) ? "Customer Detail" : "Customer Detail - #" + CustomerId;
        partial void OnCustomerIdChanged(string value)
        {
            OnPropertyChanged(nameof(PageTitle));
        }


        [RelayCommand]
        private async Task LoadCustomerDetailAsync()
        {
            try
            {
                
                if (IsBusy) return;
                IsBusy = true;

                var result = await _customerApi.GetCustomerByIdAsync(int.Parse(CustomerId),CancellationToken.None);
                
                if (result?.Data is null) 
                {
                    Toast.Make("Customer not found",CommunityToolkit.Maui.Core.ToastDuration.Long);
                    return;
                }
                
                FirstName = result.Data.FirstName;
                LastName = result.Data.LastName;
                CustomerName= result.Data.CustomerName;
                Email= result.Data.Email;
                Phone=result.Data.Phone;
                Image = result.Data.Image;
                Street = result.Data.Street;
                City = result.Data.City;
                Phone = result.Data.Phone;
                ZipCode= result.Data.ZipCode;
                State = result.Data.State;

            }
            catch (Exception ex)
            {
                Toast.Make("Internal Error", CommunityToolkit.Maui.Core.ToastDuration.Long);
            }
            finally {
                IsBusy = false;
            }
            

        }


        [RelayCommand]
        private async Task GoToEditPageAsync() {

            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "customer", new CustomerDetailDto{ 
                    CustomerName=CustomerName,
                    CustomerId=int.Parse(CustomerId),
                    FirstName=FirstName,
                    LastName=LastName,
                    Email=Email,
                    Phone=Phone,
                    Street=Street,
                    City=City,
                    State=State,
                    ZipCode=ZipCode,
                    Image=Image
                    } }
                };

                await Shell.Current.GoToAsync(nameof(AddEditCustomer),true, parameters);
            }
            catch (Exception ex)
            {
                
                
            }
        }

    }
}

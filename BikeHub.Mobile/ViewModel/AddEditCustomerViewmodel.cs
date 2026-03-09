using BikeHub.Mobile.ApiServices;
using BikeHub.Mobile.Pages;
using BikeHub.Shared.Dto.Response;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Google.Crypto.Tink.Shaded.Protobuf;

namespace BikeHub.Mobile.ViewModel
{
    public partial class AddEditCustomerViewmodel : ObservableObject, IQueryAttributable
    {
        private readonly ICustomerApi _customerApi;
        public AddEditCustomerViewmodel(ICustomerApi api)
        {
            this._customerApi = api;
        }

        [ObservableProperty]
        private CustomerDetailDto _PendingcustomerToEdit;
        [ObservableProperty]
        private string _customerId;
        [ObservableProperty]
        private string _firstName;
        [ObservableProperty]
        private string _lastName;
        [ObservableProperty]
        private string _customerName;
        [ObservableProperty]
        private string _email;
        [ObservableProperty]
        private string _phone;
        [ObservableProperty]
        private string _street;
        [ObservableProperty]
        private string _city;
        [ObservableProperty]
        private string _state;
        [ObservableProperty]
        private string _zipCode;
        [ObservableProperty]
        private string _Image;

        [ObservableProperty]
        private bool _isBusy;
        
        


        private FileResult? CustomerImageFile;
        private readonly ICustomerApi api;

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                if (query.TryGetValue("customer", out var value) && value is CustomerDetailDto dto)
                {
                    _PendingcustomerToEdit = dto;
                    BindValue(_PendingcustomerToEdit);
                }

                else { 
                ClearBindedValue();
                }
            }
            catch (Exception ex)
            {

                
            }
        }
        public string PageTitle
        {
            get
            {

                if (!string.IsNullOrEmpty(CustomerId) && CustomerId!= "0")
                    return "Edit Product";

                return "Add Product";
            }
        }


        partial void OnCustomerIdChanged(string value)
        {
            
        }
        private void ClearBindedValue() 
        {
            CustomerId = string.Empty;
            CustomerName = string.Empty;
            FirstName =string.Empty;
            LastName = string.Empty;
            City = string.Empty;
            Street = string.Empty;
            ZipCode = string.Empty;
            State = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            Image = string.Empty;
        }

        private void BindValue(CustomerDetailDto dto)
        {
            CustomerId=dto?.CustomerId.ToString();
            CustomerName=dto?.CustomerName;
            FirstName=dto?.FirstName;
            LastName=dto?.LastName;
            City=dto?.City;
            Street=dto?.Street;
            ZipCode=dto?.ZipCode;
            State=dto?.State;
            Phone=dto?.Phone;
            Email=dto?.Email;
            Image=dto?.Image;
        }

        [RelayCommand]
        private async Task ChoosePhotoAsync()
        {
            try
            {
                // Pick photo from gallery
                var result = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Select a Customer Image"
                });

                if (result != null)
                {
                    // Convert to file path for Image.Source binding
                    Image = result.FullPath;
                    CustomerImageFile = result;
                }
            }
            catch (Exception ex)
            {
                
                Debug.WriteLine(ex.Message);
            }
        }


        [RelayCommand]
        private async Task SaveCustomerAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(CustomerId) && CustomerId != "0")
                {



                    var content = new MultipartFormDataContent();
                    content.Add(new StringContent(CustomerId), "Id");
                    
                    content.Add(new StringContent(FirstName), "FirstName");
                    content.Add(new StringContent(LastName), "LastName");
                    content.Add(new StringContent(Phone), "Phone");
                    content.Add(new StringContent(Email), "Email");
                    content.Add(new StringContent(Street), "Street");
                    content.Add(new StringContent(State), "State");
                    content.Add(new StringContent(City), "City");
                    content.Add(new StringContent(ZipCode), "ZipCode");


                    if (CustomerImageFile != null)
                    {
                         var stream = await CustomerImageFile.OpenReadAsync();
                        var fileContent = new StreamContent(stream);
                        fileContent.Headers.ContentType =
                            new System.Net.Http.Headers.MediaTypeHeaderValue(CustomerImageFile.ContentType ?? "application/octet-stream");
                        content.Add(fileContent, "ImageFile", CustomerImageFile?.FileName);
                    }

                    var result = await _customerApi.UpdateCustomerAsync(content, CancellationToken.None);

                    if (result.Status == true)
                    {
                        await Shell.Current.DisplayAlert("Success", "Customer saved successfully", "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", result.Message ?? "An error occurred", "OK");
                    }
                }
                else
                {
                    if (CustomerImageFile == null)
                    {
                        await Shell.Current.DisplayAlert("Error", "Please select a logo", "OK");
                        return;
                    }

                     var stream = await CustomerImageFile.OpenReadAsync();

                    var content = new MultipartFormDataContent();

                    
                    content.Add(new StringContent(FirstName), "FirstName");
                    content.Add(new StringContent(LastName), "LastName");
                    content.Add(new StringContent(Phone), "Phone");
                    content.Add(new StringContent(Email), "Email");
                    content.Add(new StringContent(Street), "Street");
                    content.Add(new StringContent(State), "State");
                    content.Add(new StringContent(City), "City");
                    content.Add(new StringContent(ZipCode), "ZipCode");
                    var fileContent = new StreamContent(stream);
                    fileContent.Headers.ContentType =
                        new System.Net.Http.Headers.MediaTypeHeaderValue(CustomerImageFile.ContentType ?? "application/octet-stream");
                    content.Add(fileContent, "ImageFile", CustomerImageFile?.FileName);


                    var result = await _customerApi.AddCustomerAsync(content, CancellationToken.None);

                    if (result.Status == true)
                    {
                        await Shell.Current.DisplayAlert("Success", "Customer saved successfully", "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", result.Message ?? "An error occurred", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");

            }

        }


    }
}

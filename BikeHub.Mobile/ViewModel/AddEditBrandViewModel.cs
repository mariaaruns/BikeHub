using BikeHub.Mobile.ApiServices;
using BikeHub.Mobile.Pages;
using BikeHub.Shared.Dto.Request;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Mobile.ViewModel
{
    [QueryProperty(nameof(BrandId), "brandId")]
    [QueryProperty(nameof(BrandName), "brandName")]
    [QueryProperty(nameof(BrandLogo), "brandLogo")]
    public partial class AddEditBrandViewModel : ObservableObject
    {
        private readonly IProductApi _productApi;
        public AddEditBrandViewModel(IProductApi productApi)
        {
            this._productApi = productApi;
        }
        [ObservableProperty]
        private string brandId;

        [ObservableProperty]
        private string _brandName;

        [ObservableProperty]
        private string _brandLogo;

        public FileResult BrandLogoFile { get; set; }
        public string PageTitle
        {
            get
            {
                if (!string.IsNullOrEmpty(BrandId) && BrandId != "0")
                    return "Edit Brand";

                return "Add Brand";
            }
        }


        private void RaisePageTitleChanged() => OnPropertyChanged(nameof(PageTitle));


        partial void OnBrandIdChanged(string value) => RaisePageTitleChanged();

   
        [RelayCommand]
        private async Task SaveBrandAsync()
        {

            try
            {

                
                if (BrandLogoFile == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Please select a logo", "OK");
                    return;
                }

                using var stream = await BrandLogoFile.OpenReadAsync();

                var content = new MultipartFormDataContent();

                // Text field
                content.Add(
                    new StringContent(BrandName),
                    "BrandName"
                );

                // File field (THIS maps to IFormFile)
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

                content.Add(
                    fileContent,
                    "BrandImage",           // MUST match API parameter name
                    BrandLogoFile?.FileName
                );

                if (brandId is "0")
                {
                    var result = await _productApi.AddBrandAsync(content, CancellationToken.None);
                    if (result.Status == true)
                        await Shell.Current.GoToAsync("..");

                    else
                        await Shell.Current.DisplayAlert("Validation", "Brand name required", "Ok");
                }

                else {

                    var result = await _productApi.AddBrandAsync(content, CancellationToken.None);
                    
                    if (result.Status == true)
                        await Shell.Current.GoToAsync("..");

                    else
                        await Shell.Current.DisplayAlert("Validation", "Brand name required", "Ok");
                }
            }
            catch(Exception ex) {
                await Shell.Current.DisplayAlert("Internal Error",$"{ex.Message}","Ok");
            }
        }
        [RelayCommand]
        private async Task PickImageAsync()
        {
            try
            {
                // Pick photo from gallery
                var result = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Select a Brand Logo"
                });

                if (result != null)
                {
                    // Convert to file path for Image.Source binding
                    BrandLogo = result.FullPath;
                    BrandLogoFile = result;
                }
            }
            catch (Exception ex)
            {
                // Handle errors (user cancel, permission denied, etc.)
                Console.WriteLine(ex.Message);
            }
        }
    }
}

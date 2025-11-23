using BikeHub.Mobile.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Mobile.ViewModel
{
    [QueryProperty(nameof(ProductId), "productId")]
   public partial class AddEditProductViewModel:ObservableObject
    {
        [ObservableProperty]
        private string productId;
        public string PageTitle
        {
            get
            {

                if (!string.IsNullOrEmpty(ProductId) && ProductId != "0")
                    return "Edit Product";

                return "Add Product";
            }
        }
        [ObservableProperty]
        private string productName;
        [ObservableProperty] private string productImage;
        [ObservableProperty] private string price;
        [ObservableProperty] private string quantity;

        [ObservableProperty] private string selectedBrand;
        public List<string> BrandList { get; } = new List<string> { "Brand1", "Brand2" };

        [ObservableProperty] private string selectedCategory;
        public List<string> CategoryList { get; } = new List<string> { "Cat1", "Cat2" };


        private void RaisePageTitleChanged() => OnPropertyChanged(nameof(PageTitle));

        partial void OnProductIdChanged(string value) => RaisePageTitleChanged();


        [RelayCommand]
        private async Task UploadPhotoAsync()
        {
            try
            {
                // Pick photo from gallery
                var result = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Select a Product Image"
                });

                if (result != null)
                {
                    // Convert to file path for Image.Source binding
                    ProductImage = result.FullPath;
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

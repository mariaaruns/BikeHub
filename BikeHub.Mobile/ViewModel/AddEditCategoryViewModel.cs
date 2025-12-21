using BikeHub.Mobile.ApiServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Mobile.ViewModel
{

    [QueryProperty(nameof(CategoryId), "categoryId")]
    [QueryProperty(nameof(CategoryName), "categoryName")]
    public partial class AddEditCategoryViewModel : ObservableObject
    {
        private readonly IProductApi _productApi;

        public AddEditCategoryViewModel(IProductApi productApi)
        {
            _productApi = productApi;
        }

        [ObservableProperty]
        private string _categoryId;

        [ObservableProperty]
        private string _categoryName;

        public string PageTitle
        {
            get
            {
                if (!string.IsNullOrEmpty(CategoryId) && CategoryId != "0")
                    return "Edit Category";

                return "Add Category";
            }
        }

        private void RaisePageTitleChanged() => OnPropertyChanged(nameof(PageTitle));

        partial void OnCategoryIdChanged(string value) => RaisePageTitleChanged();


        [RelayCommand]
        private async Task SaveAsync()
        {
            if (string.IsNullOrEmpty(CategoryId))
            {
                //Categoryid is 0 add else edit
                var result = await _productApi.AddCategoryAsync(new Shared.Dto.Request.AddCategoryDto
                {
                    CategoryName = CategoryName
                }, CancellationToken.None);

                if (result.Status) { 
              await App.Current.MainPage.DisplayAlert("Success", "Category added successfully", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", result.Message ?? "An error occurred", "OK");
                }

            }
            else
            {
                var result = await _productApi.UpdateCategoryAsync(new Shared.Dto.Request.UpdateCategoryDto
                {
                    CategoryId = int.Parse(CategoryId),
                    CategoryName = CategoryName
                }, CancellationToken.None);


                if (result.Status) { 
                
                    await App.Current.MainPage.DisplayAlert("Success", "Category updated successfully", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", result.Message ?? "An error occurred", "OK");
                }
            }
        }


    }
}


using BikeHub.Mobile.ApiServices;
using BikeHub.Mobile.Pages;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
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



    public partial class ProductsViewModel : ObservableObject
    {



        [ObservableProperty]
        private ObservableCollection<ProductsDto> _products = new();
        [ObservableProperty]
        public ObservableCollection<BrandsDto> _brands = new();
        [ObservableProperty]
        public ObservableCollection<CategoryDto> _categories = new();
        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private bool _isBrandRefreshing;

        [ObservableProperty]
        private bool _isCategoryRefreshing;


        [ObservableProperty]
        private string productName;

        [ObservableProperty]
        private string categoryName;

        [ObservableProperty]
        private string brandName;


        [ObservableProperty]
        private bool _isLoadingMore;
        [ObservableProperty]
        private bool _isBrandBusy;

        [ObservableProperty]
        private bool _isCategoryBusy;

        private CancellationTokenSource _searchCts;
        async partial void OnProductNameChanged(string newValue)
        {
            try
            {
                _searchCts?.Cancel();
                _searchCts = new CancellationTokenSource();
                var token = _searchCts.Token;
                await Task.Delay(500, token);

                Products.Clear();

                _ = LoadProductAsync(token);
            }
            catch (TaskCanceledException)
            {

            }
            catch (Exception)
            {

                throw;
            }


        }


        private CancellationTokenSource _searchBrandCts;
        async partial void OnBrandNameChanged(string newValue)
        {
            try
            {
                _searchBrandCts?.Cancel();
                _searchBrandCts = new CancellationTokenSource();
                var token = _searchBrandCts.Token;
                await Task.Delay(500, token);

                Brands.Clear();

                _ = LoadBrandAsync(token);
            }
            catch (TaskCanceledException)
            {

            }
            catch (Exception)
            {

                throw;
            }


        }



        private CancellationTokenSource _searchCategoryCts;
        async partial void OnCategoryNameChanged(string newValue)
        {
            try
            {
                _searchCategoryCts?.Cancel();
                _searchCategoryCts = new CancellationTokenSource();
                var token = _searchCategoryCts.Token;
                await Task.Delay(500, token);

                Categories.Clear();

                //_ = LoadBrandAsync(token);
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


        // bind to Image.Source
        private readonly IProductApi _productApi;
        public ProductsViewModel(IProductApi productApi)
        {
            _productApi = productApi;
        }



        [RelayCommand]
        public async Task DeleteBrandsAsync(BrandsDto brand)
        {
            if (brand is null)
                return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Delete Brand",
                $"Are you sure you want to delete {brand.BrandName}?",
                "Yes", "No");

            if (confirm && Brands.Contains(brand))
                Brands.Remove(brand);
        }

        [RelayCommand]
        public async Task DeleteProductAsync(ProductsDto product)
        {
            if (product is null)
                return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Delete product",
                $"Are you sure you want to delete {product.ProductName}?",
                "Yes", "No");

            if (confirm && Products.Contains(product))
                Products.Remove(product);
        }


        [RelayCommand]
        public async Task DeleteCategoryAsync(CategoryDto category)
        {
            if (category is null)
                return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Delete category",
                $"Are you sure you want to delete {category.CategoryName}?",
                "Yes", "No");

            if (confirm && Categories.Contains(category))
            {
               var result= await _productApi.DeleteCategoryByIdAsync(category.CategoryId,CancellationToken.None);
                if(result.Status)
                Categories.Remove(category);

            }
        }



        [RelayCommand]
        public async Task GotoAddProductAsync(ProductsDto? dto)
        {
            await Shell.Current.GoToAsync($"{nameof(AddEditProductPage)}?productId={dto?.ProductId}");
        }


        [RelayCommand]
        public async Task GotoAddBrandAsync(BrandsDto? dto)
        {

            await Shell.Current.GoToAsync($"{nameof(AddEditBrand)}?brandId={dto?.BrandId}");
        }

        [RelayCommand]
        public async Task GotoAddCategory(CategoryDto? dto)
        {
            await Shell.Current.GoToAsync($"{nameof(AddEditCategory)}?categoryId={dto?.CategoryId}&categoryName={dto?.CategoryName}");
        }

        [RelayCommand]
        public async Task LoadProductAsync(CancellationToken cancellationToken)
        {

            try
            {
                if (IsLoadingMore) return;
                IsLoadingMore = true;

                var dto = new GetProductsDto
                {

                    PageNumber = _currentPage,
                    PageSize = _pageSize,
                    ProductNameFilter = ProductName
                };

                var result = await _productApi.GetProductsAsync(dto, cancellationToken);

                if (result is null)
                {
                    IsLoadingMore = false;
                    return;
                }

                if (result.Status)
                    result.Data.Data.ForEach(p => Products.Add(p));

                _currentPage++;
            }
            catch (TaskCanceledException)
            {

            }
            catch (Exception ex)
            {
                var toast = Toast.Make("Unexpected Error , While Product Load", ToastDuration.Long);
                await toast.Show(cancellationToken);

            }
            finally
            {

                IsLoadingMore = false;

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
                Products.Clear();
                await LoadProductAsync(cts.Token);
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
        public async Task LoadBrandAsync(CancellationToken cancellationToken)
        {

            try
            {
                if (IsBrandBusy) return;
                IsBrandBusy = true;



                var result = await _productApi.GetAllBrandsAsync(brandName, cancellationToken);

                if (result is null)
                {
                    IsLoadingMore = false;
                    return;
                }
                Brands.Clear();
                if (result.Status)
                    foreach (var brand in result.Data)
                    {
                        Brands.Add(brand);
                    }



            }
            catch (TaskCanceledException)
            {

            }
            catch (Exception ex)
            {
                var toast = Toast.Make("Unexpected Error , While Brand Load", ToastDuration.Long);
                await toast.Show(cancellationToken);

            }
            finally
            {

                IsBrandBusy = false;

            }
        }

        [RelayCommand]
        private async Task RefreshBrandAsync()
        {

            IsBrandRefreshing = true;

            try
            {

                _currentPage = 1; // Reset to the first page
                var cts = new CancellationTokenSource();
                Brands.Clear();
                await LoadBrandAsync(cts.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Refresh Failed: {ex.Message}");
            }
            finally
            {

                IsBrandRefreshing = false;
            }
        }

        [RelayCommand]
        public async Task LoadCategoryAsync(CancellationToken cancellationToken)
        {

            try
            {
                if (IsCategoryBusy) return;
                IsCategoryBusy = true;



                var result = await _productApi.GetAllCategoriesAsync(CategoryName, cancellationToken);

                if (result is null)
                {
                    IsLoadingMore = false;
                    return;
                }
                
                if (result.Status)
                    foreach (var category in result.Data)
                    {
                        Categories.Add(category);
                    }



            }
            catch (TaskCanceledException)
            {

            }
            catch (Exception ex)
            {
                var toast = Toast.Make("Unexpected Error , While Category Load", ToastDuration.Long);
                await toast.Show(cancellationToken);

            }
            finally
            {

                IsCategoryBusy = false;

            }
        }

        [RelayCommand]
        private async Task RefreshCategoryAsync()
        {

            IsCategoryRefreshing = true;

            try
            {

            
                var cts = new CancellationTokenSource();
                Categories.Clear();
                await LoadCategoryAsync(cts.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Refresh Failed: {ex.Message}");
            }
            finally
            {

                IsCategoryRefreshing = false;
            }
        }

        [RelayCommand]
        private async Task LoadAllDataAsync(CancellationToken cancellationToken)
        {
            try
            {
                var task1 = LoadProductAsync(cancellationToken);
                var task2 = LoadBrandAsync(cancellationToken);
                var task3 = LoadCategoryAsync(cancellationToken);
                await Task.WhenAll(task1, task2,task3);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Load All Data Failed: {ex.Message}");
            }
            

        }
    }
}

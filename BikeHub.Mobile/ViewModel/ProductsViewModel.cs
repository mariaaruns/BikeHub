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
    [QueryProperty(nameof(ProductId), "productId")]
    [QueryProperty(nameof(BrandId), "brandId")]
    [QueryProperty(nameof(CategoryId), "categoryId")]
    public partial class ProductsViewModel:ObservableObject
    {
        [ObservableProperty]
        private string productId;
        [ObservableProperty]
        private string categoryId;
        [ObservableProperty]
        private string brandId;

        [ObservableProperty]
        private bool _isRefreshing;
        public string PageTitle
        {
            get
            {

                if (!string.IsNullOrEmpty(ProductId) && ProductId != "0")
                    return "Edit Product";


                if (!string.IsNullOrEmpty(BrandId) && BrandId != "0")
                    return "Edit Brand";

                if (!string.IsNullOrEmpty(CategoryId) && CategoryId != "0")
                    return "Edit Category";

                
                if (string.IsNullOrEmpty(ProductId) || ProductId == "0")
                    return "Add Product";

                if (string.IsNullOrEmpty(BrandId) || BrandId == "0")
                    return "Add Brand";

                if (string.IsNullOrEmpty(CategoryId) || CategoryId == "0")
                    return "Add Category";

                return "Add Item";
            }
        }



        private void RaisePageTitleChanged() => OnPropertyChanged(nameof(PageTitle));

        partial void OnProductIdChanged(string value) => RaisePageTitleChanged();
        partial void OnBrandIdChanged(string value) => RaisePageTitleChanged();
        partial void OnCategoryIdChanged(string value) => RaisePageTitleChanged();


        [ObservableProperty]
        private ObservableCollection<ProductsDto> _products=new();
        public ObservableCollection<BrandsDto> Brands { get; set; }
        public ObservableCollection<CategoryDto> Categories { get; set; }
        [ObservableProperty]
        private string productName;
        [ObservableProperty] private string price;
        [ObservableProperty] private string quantity;

        [ObservableProperty]
        private bool _isLoadingMore;

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
            catch (TaskCanceledException) { 
            
            }
            catch (Exception)
            {

                throw;
            }
         
        
        }

        private int _currentPage = 1;
        private int _pageSize = 20;

        [ObservableProperty] private string selectedBrand;
        public List<string> BrandList { get; } = new List<string> { "Brand1", "Brand2" };

        [ObservableProperty] private string selectedCategory;
        public List<string> CategoryList { get; } = new List<string> { "Cat1", "Cat2" };

        
        
        [RelayCommand] 
        private async Task UploadPhotoAsync() {

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

        [ObservableProperty] private string productImage; // bind to Image.Source
        private readonly IProductApi _productApi;
        public ProductsViewModel(IProductApi productApi)
        {
            _productApi = productApi;

            //Brands = new ObservableCollection<BrandsDto>();

            //Brands.Add(new BrandsDto() { BrandId = 1, BrandName = "Trek", Logo = "woman.png" });
            //Brands.Add(new BrandsDto() { BrandId = 2, BrandName = "Giant", Logo = "woman.png" });
            //Brands.Add(new BrandsDto() { BrandId = 3, BrandName = "Specialized", Logo = "woman.png" });
            //Brands.Add(new BrandsDto() { BrandId = 4, BrandName = "Cannondale", Logo = "woman.png" });
            //Brands.Add(new BrandsDto() { BrandId = 5, BrandName = "Scott", Logo = "woman.png" });
            //Brands.Add(new BrandsDto() { BrandId = 6, BrandName = "Bianchi", Logo = "woman.png" });
            //Brands.Add(new BrandsDto() { BrandId = 7, BrandName = "Santa Cruz", Logo = "woman.png" });
            //Brands.Add(new BrandsDto() { BrandId = 8, BrandName = "Merida", Logo = "woman.png" });
            //Brands.Add(new BrandsDto() { BrandId = 9, BrandName = "Cube", Logo = "woman.png" });
            //Brands.Add(new BrandsDto() { BrandId = 10, BrandName = "Polygon", Logo = "woman.png" });


            //Categories = new ObservableCollection<CategoryDto>();
            //Categories.Add(new CategoryDto() { CategoryId = 1, CategoryName = "Mountain Bikes" });
            //Categories.Add(new CategoryDto() { CategoryId = 2, CategoryName = "Road Bikes" });
            //Categories.Add(new CategoryDto() { CategoryId = 3, CategoryName = "Hybrid Bikes" });
            //Categories.Add(new CategoryDto() { CategoryId = 4, CategoryName = "Electric Bikes" });
            //Categories.Add(new CategoryDto() { CategoryId = 5, CategoryName = "Kids Bikes" });
            //Categories.Add(new CategoryDto() { CategoryId = 6, CategoryName = "BMX Bikes" });
            //Categories.Add(new CategoryDto() { CategoryId = 7, CategoryName = "Folding Bikes" });
            //Categories.Add(new CategoryDto() { CategoryId = 8, CategoryName = "Gravel Bikes" });
            //Categories.Add(new CategoryDto() { CategoryId = 9, CategoryName = "Cruiser Bikes" });
            //Categories.Add(new CategoryDto() { CategoryId = 10, CategoryName = "Track Bikes" });

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
                Categories.Remove(category);
        }



        [RelayCommand]
        public async Task GotoAddProduct() {

            await Shell.Current.GoToAsync($"{nameof(AddEditProductPage)}?productId=0&brandId={brandId}&categoryId={categoryId}");
        
        }


        [RelayCommand]
        public async Task GotoAddBrand()
        {

            await Shell.Current.GoToAsync($"{nameof(AddEditBrand)}?productId={ProductId}&brandId=0&categoryId={categoryId}");

        }


        [RelayCommand]
        public async Task GotoAddCategory()
        {
            await Shell.Current.GoToAsync($"{nameof(AddEditCategory)}?productId={ProductId}&brandId={brandId}&categoryId=0");
        }


        [RelayCommand]
        public async Task LoadProductAsync(CancellationToken cancellationToken) {

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

                var result = await _productApi.GetProducts(dto, cancellationToken);

                if (result is null)
                {
                    IsLoadingMore = false;
                    return;
                }

                if (result.Status)
                    result.Data.Data.ForEach(p => Products.Add(p));

                _currentPage++;
            }
            catch (TaskCanceledException) { 
            
            }
            catch (Exception)
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
        private async Task RefreshAsync() {

            IsRefreshing = true;

            try
            {
                // 2. RESET Pagination Counters
                _currentPage = 1; // Reset to the first page

                // 3. Create a fresh token for this request
                // We don't use the SearchCancellationToken here because this is an explicit user action
                var cts = new CancellationTokenSource();

                // 4. Call your API
                // IMPORTANT: You usually want to clear the list to remove old data
                // or you can wait until data loads to avoid a "blank screen" flash.
                Products.Clear();

                await LoadProductAsync(cts.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Refresh Failed: {ex.Message}");
            }
            finally
            {
                // 5. CRITICAL: You must turn off the spinner, 
                // otherwise it spins forever.
                IsRefreshing = false;
            }
        }

    }
}

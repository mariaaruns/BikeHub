using BikeHub.Mobile.Pages;
using BikeHub.Shared.Dto.Response;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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



        public ObservableCollection<ProductsDto> Products { get; set; }
        public ObservableCollection<BrandsDto> Brands { get; set; }
        public ObservableCollection<CategoryDto> Categories { get; set; }
        [ObservableProperty] private string productName;
        [ObservableProperty] private string price;
        [ObservableProperty] private string quantity;

        
        
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
        public ProductsViewModel()
        {
            Products = new ObservableCollection<ProductsDto>();
            Products.Add(new ProductsDto() { ProductId = 1, ProductName = "Trek Fuel EX 9.8", CategoryName = "Mountain Bike", Stock = 12, Price = 2000.90m, ProductImage = "mountain_bike.png" });
            Products.Add(new ProductsDto() { ProductId = 2, ProductName = "Specialized Turbo Levo", CategoryName = "Electric Bike", Stock = 8, Price = 4800.50m, ProductImage = "mountain_bike.png" });
            Products.Add(new ProductsDto() { ProductId = 3, ProductName = "Giant Defy Advanced 2", CategoryName = "Road Bike", Stock = 15, Price = 2100.00m, ProductImage = "mountain_bike.png" });
            Products.Add(new ProductsDto() { ProductId = 4, ProductName = "Cannondale Quick 4", CategoryName = "Hybrid Bike", Stock = 20, Price = 950.75m, ProductImage = "mountain_bike.png" });
            Products.Add(new ProductsDto() { ProductId = 5, ProductName = "Santa Cruz Hightower", CategoryName = "Mountain Bike", Stock = 5, Price = 3700.25m, ProductImage = "mountain_bike.png" });
            Products.Add(new ProductsDto() { ProductId = 6, ProductName = "Bianchi Oltre XR4", CategoryName = "Road Bike", Stock = 10, Price = 5600.00m, ProductImage = "mountain_bike.png" });
            Products.Add(new ProductsDto() { ProductId = 7, ProductName = "Polygon Path X5", CategoryName = "Hybrid Bike", Stock = 18, Price = 750.40m, ProductImage = "mountain_bike.png" });
            Products.Add(new ProductsDto() { ProductId = 8, ProductName = "Yamaha CrossCore RC", CategoryName = "Electric Bike", Stock = 6, Price = 3200.10m, ProductImage = "mountain_bike.png" });
            Products.Add(new ProductsDto() { ProductId = 9, ProductName = "Marin Rift Zone 29 3", CategoryName = "Mountain Bike", Stock = 9, Price = 2450.60m, ProductImage = "mountain_bike.png" });
            Products.Add(new ProductsDto() { ProductId = 10, ProductName = "Trek FX 3 Disc", CategoryName = "Hybrid Bike", Stock = 14, Price = 1200.00m, ProductImage = "mountain_bike.png" });

             Brands = new ObservableCollection<BrandsDto>();

            Brands.Add(new BrandsDto() { BrandId = 1, BrandName = "Trek", Logo = "woman.png" });
            Brands.Add(new BrandsDto() { BrandId = 2, BrandName = "Giant", Logo = "woman.png" });
            Brands.Add(new BrandsDto() { BrandId = 3, BrandName = "Specialized", Logo = "woman.png" });
            Brands.Add(new BrandsDto() { BrandId = 4, BrandName = "Cannondale", Logo = "woman.png" });
            Brands.Add(new BrandsDto() { BrandId = 5, BrandName = "Scott", Logo = "woman.png" });
            Brands.Add(new BrandsDto() { BrandId = 6, BrandName = "Bianchi", Logo = "woman.png" });
            Brands.Add(new BrandsDto() { BrandId = 7, BrandName = "Santa Cruz", Logo = "woman.png" });
            Brands.Add(new BrandsDto() { BrandId = 8, BrandName = "Merida", Logo = "woman.png" });
            Brands.Add(new BrandsDto() { BrandId = 9, BrandName = "Cube", Logo = "woman.png" });
            Brands.Add(new BrandsDto() { BrandId = 10, BrandName = "Polygon", Logo = "woman.png" });


            Categories = new ObservableCollection<CategoryDto>();
            Categories.Add(new CategoryDto() { CategoryId = 1, CategoryName = "Mountain Bikes" });
            Categories.Add(new CategoryDto() { CategoryId = 2, CategoryName = "Road Bikes" });
            Categories.Add(new CategoryDto() { CategoryId = 3, CategoryName = "Hybrid Bikes" });
            Categories.Add(new CategoryDto() { CategoryId = 4, CategoryName = "Electric Bikes" });
            Categories.Add(new CategoryDto() { CategoryId = 5, CategoryName = "Kids Bikes" });
            Categories.Add(new CategoryDto() { CategoryId = 6, CategoryName = "BMX Bikes" });
            Categories.Add(new CategoryDto() { CategoryId = 7, CategoryName = "Folding Bikes" });
            Categories.Add(new CategoryDto() { CategoryId = 8, CategoryName = "Gravel Bikes" });
            Categories.Add(new CategoryDto() { CategoryId = 9, CategoryName = "Cruiser Bikes" });
            Categories.Add(new CategoryDto() { CategoryId = 10, CategoryName = "Track Bikes" });

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
    }
}

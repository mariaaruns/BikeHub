using Android.OS;
using Android.Webkit;
using BikeHub.Mobile.ApiServices;
using BikeHub.Mobile.Pages;
using BikeHub.Shared.Dto.Response;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;


namespace BikeHub.Mobile.ViewModel
{
   
    public partial class AddEditProductViewModel:ObservableObject, IQueryAttributable
    {
        private readonly IProductApi _productApi;
        public AddEditProductViewModel(IProductApi productApi)
        {
            this._productApi = productApi;    
        }
        private ProductsDto _pendingProductToEdit;
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                if (query.TryGetValue("Product", out var value) && value is ProductsDto product)
                {
                    //BindFromProduct(product);
                    _pendingProductToEdit = product;
                }
                else
                {
                    ClearBindedValue();
                }
            }
            catch (Exception ex)
            {
                 Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private void BindFromProduct(ProductsDto product)
        {
            ProductId = product.ProductId.ToString();
            ProductName = product.ProductName;
            Price = product.Price?.ToString();
            Quantity = product.Stock?.ToString();
            //ProductImage = product.ProductImage;
            SelectedModelYear = int.Parse(product.ModelYear);
            SelectedCategory = CategoryList.FirstOrDefault(x=>x.Value==product.CategoryId);
            SelectedBrand = BrandList.FirstOrDefault(x => x.Value == product.BrandId);

            if (!string.IsNullOrWhiteSpace(product.ProductImageUrl))
            {
                ProductImage = ImageSource.FromUri(
                    new Uri(product.ProductImageUrl)
                );
            }
            else
            {
                ProductImage = "man.png";
            }
        }
        private void ClearBindedValue() {

            ProductId = string.Empty;
            ProductName = string.Empty;
            Price = string.Empty;
            Quantity = string.Empty;
            //ProductImage = product.ProductImage;
            SelectedCategory = null;
            selectedBrand = null;
            SelectedModelYear = 0;
            ProductImage = string.Empty;
            ProductImageFile = null;

        }

        [ObservableProperty]
        private ProductsDto product;



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
        
        [ObservableProperty]
        private ImageSource productImage;
        
        [ObservableProperty]
        private FileResult? productImageFile;

        [ObservableProperty]
        private string price;
        
        [ObservableProperty]
        private string quantity;
        
        [ObservableProperty] 
        private DropdownDto? selectedBrand;
        
        [ObservableProperty]
        private int selectedModelYear;

        [ObservableProperty]
        private ObservableCollection<int> _modelyearList = new(Enumerable.Range(2015, DateTime.Now.Year - 2014));
        
        [ObservableProperty]
        private ObservableCollection<DropdownDto> _brandList = new();
        
        [ObservableProperty] 
        private DropdownDto? selectedCategory;

        [ObservableProperty]
        private ObservableCollection<DropdownDto> _categoryList  = new();
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
                    ProductImageFile = result;
                }
            }
            catch (Exception ex)
             {
                // Handle errors (user cancel, permission denied, etc.)
                Console.WriteLine(ex.Message);
            }
        }
       
        [RelayCommand]        
        private async Task LoadDropDownAsync() 
        {
            try
            {
                var task1 = LoadBrandDropdownAsync();
                var task2 = LoadCategoryDropdownAsync();
                await Task.WhenAll(task1,task2);

                if (_pendingProductToEdit != null) 
                {
                    BindFromProduct(_pendingProductToEdit);
                    _pendingProductToEdit = null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task LoadBrandDropdownAsync()
        {
            try
            {
                var result = await _productApi.GetDropdownAsync("Brand",CancellationToken.None);
             
                if(result.Status && result.Data != null)
                {
                    BrandList.Clear();
                    foreach (var item in result.Data)
                    {
                        BrandList.Add(new DropdownDto { Text = item.Text, Value = item.Value });
                    }
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task LoadCategoryDropdownAsync()
        {
            try
            {
                var result = await _productApi.GetDropdownAsync("Category", CancellationToken.None);

                if (result.Status && result.Data != null)
                {
                    CategoryList.Clear();
                    foreach (var item in result.Data)
                    {
                        CategoryList.Add(new DropdownDto {Text=item.Text,Value=item.Value });
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
     
        [RelayCommand]
        private async Task SaveProductAsync() 
        {
            try
            {
                if (!string.IsNullOrEmpty(ProductId) && ProductId!="0")
                {
                    
                  

                    var content = new MultipartFormDataContent();
                    content.Add(new StringContent(ProductId), "ProductId");
                    content.Add(new StringContent(ProductName), "ProductName");
                    content.Add(new StringContent(SelectedBrand.Value.ToString()), "BrandId");
                    content.Add(new StringContent(SelectedCategory.Value.ToString()), "CategoryId");
                    content.Add(new StringContent(SelectedModelYear.ToString()), "ModelYear");
                    content.Add(new StringContent(Price.ToString()), "ListPrice");
                    content.Add(new StringContent(Quantity.ToString()), "StockQty");
                  

                    if (ProductImageFile != null)
                    {
                        using var stream = await ProductImageFile.OpenReadAsync();
                        var fileContent = new StreamContent(stream);
                        fileContent.Headers.ContentType =
                            new System.Net.Http.Headers.MediaTypeHeaderValue(ProductImageFile.ContentType ?? "application/octet-stream");
                        content.Add(fileContent, "ProductImage", ProductImageFile?.FileName);
                    }

                    var result = await _productApi.UpdateProductAsync(content, CancellationToken.None);

                    if (result.Status == true)
                    {
                        await Shell.Current.DisplayAlert("Success", "Product saved successfully", "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", result.Message ?? "An error occurred", "OK");
                    }
                }
                else
                {
                    if (ProductImageFile == null)
                    {
                        await Shell.Current.DisplayAlert("Error", "Please select a logo", "OK");
                        return;
                    }

                    using var stream = await ProductImageFile.OpenReadAsync();

                    var content = new MultipartFormDataContent();

                    content.Add(new StringContent(ProductName), "ProductName");
                    content.Add(new StringContent(SelectedBrand.Value.ToString()), "BrandId");
                    content.Add(new StringContent(SelectedCategory.Value.ToString()), "CategoryId");
                    content.Add(new StringContent(SelectedModelYear.ToString()), "ModelYear");
                    content.Add(new StringContent(Price.ToString()), "ListPrice");
                    content.Add(new StringContent(Quantity.ToString()), "StockQty");
                    var fileContent = new StreamContent(stream);
                    fileContent.Headers.ContentType =
                        new System.Net.Http.Headers.MediaTypeHeaderValue(ProductImageFile.ContentType ?? "application/octet-stream");
                    content.Add(fileContent, "ProductImage", ProductImageFile?.FileName);


                    var result = await _productApi.AddProductAsync(content, CancellationToken.None);

                    if (result.Status == true)
                    {
                        await Shell.Current.DisplayAlert("Success", "Product saved successfully", "OK");
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


        private async Task GoBackAsync() {

            await Shell.Current.GoToAsync("..");
        }



    }
}

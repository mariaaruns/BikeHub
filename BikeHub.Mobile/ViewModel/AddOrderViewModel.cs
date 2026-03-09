using BikeHub.Mobile.ApiServices;
using BikeHub.Mobile.Helper;
using BikeHub.Mobile.Models;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntelliJ.Lang.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Mobile.ViewModel
{
    public partial class AddOrderViewModel : ObservableObject
    {
        private readonly IProductApi _productApi;
        private readonly IOrderApi _orderApi;
        private readonly ICustomerApi _customerApi;

        public AddOrderViewModel(IProductApi productApi, IOrderApi orderApi, ICustomerApi customerApi)
        {
            _productApi = productApi;
            _orderApi = orderApi;
            _customerApi = customerApi;
        }

        
        [ObservableProperty]
        private DropdownDto _selectedOrderStatus;
        [ObservableProperty]
        private DateTime _orderDate = DateTime.Now;
        [ObservableProperty]
        private DateTime _requiredDate = DateTime.Now.AddDays(7);


        [ObservableProperty]
        private DropdownDto _selectedBrand;

        [ObservableProperty]
        private DropdownDto _selectedCategory;

        [ObservableProperty]
        private DropdownDto _selectedCustomer;


        [ObservableProperty]
        private ObservableCollection<DropdownDto> _orderStatusList = new();

        [ObservableProperty]
        private ObservableCollection<DropdownDto> _brandList = new();

        [ObservableProperty]
        private ObservableCollection<DropdownDto> _categoryList = new();

        [ObservableProperty]
        private ObservableCollection<ProductDropdownDto> _productList = new();

        [ObservableProperty]
        private ObservableCollection<OrderItemsCartDto> _orderItemsCart = new();

        [ObservableProperty]

        private ObservableCollection<DropdownDto> _customerList = new();


        [ObservableProperty]
        private decimal _totalPrice;


        [ObservableProperty]
        private decimal _discount;

        [ObservableProperty]
        private decimal _subTotal;


        [ObservableProperty]
        private bool _isBusy;


        [ObservableProperty]
        private bool _isOpen;

        [ObservableProperty]
        private bool _isCustomerSelectionOpen;


        [ObservableProperty]

        private string _searchCustomer;

        private CancellationTokenSource _customerCts;
        async partial void OnSearchCustomerChanged(string value)
        {
            try
            {
                _customerCts?.Cancel();
                _customerCts = new CancellationTokenSource();
                var canceltoken= _customerCts.Token;
                 await Task.Delay(500, canceltoken);

                _ = LoadCustomerListAsync(canceltoken);
            }
            catch (Exception)
            { 
            
            }
        }



        [RelayCommand]
        private async Task AddProductsAsync()
        {
            IsOpen = true;
            _ = LoadProductsByFilterAsync();
        }

        [RelayCommand]
        private async Task ClosePopupAsync()
        {
            IsOpen = false;
        }


        async partial void OnSelectedBrandChanged(DropdownDto value)
        {
            try
            {
                if (value is { Value: > 0 })
                    _ = LoadProductsByFilterAsync();

                else
                {
                    var toast = Toast.Make("No brand selected");
                    await toast.Show();
                }
            }
            catch (Exception)
            {


            }
        }

        async partial void OnSelectedCategoryChanged(DropdownDto value)
        {
            try
            {
                if (value is { Value: > 0 })
                    _ = LoadProductsByFilterAsync();
                else
                {
                    var toast = Toast.Make("No category selected");
                    await toast.Show();
                }
            }
            catch (Exception)
            {
            }
        }



        

        [RelayCommand]
        private async Task OpenCustomerSelectionAsync()
        {
            try
            {
                IsCustomerSelectionOpen = true;

                await LoadCustomerListAsync(CancellationToken.None);
            }
            catch (Exception)
            {
            }
        }

        [RelayCommand]
        private async Task CloseCustomerSelectionAsync()
        {
            try
            {
                IsCustomerSelectionOpen = false;

                CustomerList.Clear();
            }
            catch (Exception)
            {
            }
        }



        [RelayCommand]
        private async Task SelectCustomerAsync(DropdownDto dto)
        {
            try
            {
                if (dto is { Value: > 0 })
                {
                    SelectedCustomer = dto;
                }
                else
                {
                    var toast = Toast.Make("No customer selected");
                    await toast.Show();
                }
            }
            catch (Exception)
            {


            }

        }


        [RelayCommand]
        private async Task LoadCustomerListAsync(CancellationToken cancellationToken)
        {

            try
            {
                var customerListResponse = await _customerApi.GetCustomerDrpodown(cancellationToken: cancellationToken,search:SearchCustomer);

                if (customerListResponse.Status && customerListResponse.Data != null)
                {

                    CustomerList.Clear();

                    foreach (var item in customerListResponse.Data)
                    {
                        CustomerList.Add(item);
                    }

                }
            }
            catch (Exception)
            {

            }
            finally
            {


            }
        }

        [RelayCommand]
        private async Task LoadBrandsAndCategories()
        {
            try
            {
                var orderStatusresponse = await _orderApi.GetOrderStatusAsync(CancellationToken.None);

                if (orderStatusresponse.Status && orderStatusresponse.Data != null)
                {
                    OrderStatusList.Clear();
                    foreach (var item in orderStatusresponse.Data)
                    {
                        OrderStatusList.Add(item);
                    }
                }




                var brandResponse = await _productApi.GetDropdownAsync("Brand", CancellationToken.None);
                if (brandResponse.Status && brandResponse.Data != null)
                {
                    BrandList.Clear();

                    foreach (var item in brandResponse.Data)
                    {
                        BrandList.Add(item);
                    }

                    SelectedBrand = BrandList.FirstOrDefault();

                }
                var categoryResponse = await _productApi.GetDropdownAsync("Category", CancellationToken.None);

                if (categoryResponse.Status && categoryResponse.Data != null)
                {
                    CategoryList.Clear();

                    foreach (var item in categoryResponse.Data)
                    {
                        CategoryList.Add(item);
                    }

                    SelectedCategory = CategoryList.FirstOrDefault();
                }


            }
            catch (Exception)
            {


            }


        }

        private async Task LoadProductsByFilterAsync()
        {
            if (SelectedBrand is null)
                return;
            if (SelectedCategory is null)
                return;
            try
            {
                IsBusy = true;

                var response = await _productApi.GetProductAndStockDropDownAsync(SelectedBrand.Value, SelectedCategory.Value, CancellationToken.None);
                if (response.Status && response.Data is not null)
                {
                    ProductList.Clear();
                    foreach (var item in response.Data)
                    {
                        ProductList.Add(item);
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task AddToCartAsync(ProductDropdownDto product)
        {
            try
            {
                if (!OrderItemsCart.Any())
                {


                    var newItem = new OrderItemsCartDto
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        Price = product.Price,

                        Stock = product.StockQty,
                        Discount = 0

                    };
                    newItem.Quantity = 1;
                    newItem.QuantityChanged += RecalculateTotals;
                    newItem.RequestRemove += RemoveItem;
                    OrderItemsCart.Add(newItem);
                    RecalculateTotals();
                }

                else
                {
                    var existingItem = OrderItemsCart.FirstOrDefault(x => x.ProductId == product.ProductId);

                    if (existingItem != null)
                    {
                        await App.Current.MainPage.DisplayAlert("Info", "Product already added in cart", "OK");
                    }
                    else
                    {
                        var newItem = new OrderItemsCartDto
                        {
                            ProductId = product.ProductId,
                            ProductName = product.ProductName,
                            Price = product.Price,
                            Stock = product.StockQty,
                            Discount = 0
                        };

                        newItem.Quantity = 1;
                        newItem.QuantityChanged += RecalculateTotals;
                        newItem.RequestRemove += RemoveItem;

                        OrderItemsCart.Add(newItem);

                        RecalculateTotals();

                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public void RecalculateTotals()
        {
            SubTotal = OrderItemsCart.Sum(x => x.Price * x.Quantity);
            Discount = OrderItemsCart.Sum(x => (x.Price * x.Quantity) * (x.Discount / 100));
            TotalPrice = SubTotal - Discount;
        }

        private void RemoveItem(OrderItemsCartDto item)
        {
            item.QuantityChanged -= RecalculateTotals;
            item.RequestRemove -= RemoveItem;

            OrderItemsCart.Remove(item);
            RecalculateTotals();
        }

        [RelayCommand]
        private async Task SaveOrderAsync()
        {
            try
            {
                IsBusy = true;
                string token = await SecureStorage.GetAsync("access_token");
                var staffId = JwtHelper.GetClaim(token, "sub");

                int loggedUserId = 0;
                if (string.IsNullOrEmpty(staffId))
                {
                    var toast = Toast.Make("Staff not identified. Please login again.");
                    await toast.Show();
                    return;
                }
                else if (int.TryParse(staffId, out loggedUserId))
                {

                }

                if (SelectedCustomer is null || SelectedCustomer.Value <= 0)
                {
                    var toast = Toast.Make("Please select a customer.");
                    await toast.Show();
                    return;
                }


                if (SelectedOrderStatus is null || SelectedOrderStatus.Value <= 0)
                {
                    var toast = Toast.Make("Please select order status.");
                    await toast.Show();
                    return;
                }

                if (!OrderItemsCart.Any())
                {
                    var toast = Toast.Make("No items in the order. Please add products to the order.");
                    await toast.Show();
                    return;
                }


                //saving order logic here

                var OrderDetails = new Shared.Dto.Request.AddOrderRequest
                {
                    CustomerId = SelectedCustomer.Value,
                    OrderDate = _orderDate,
                    RequiredDate = _requiredDate,
                    OrderStatus = SelectedOrderStatus.Value,
                    StaffId = loggedUserId,
                    OrderItemRequests = OrderItemsCart.Select(x => new OrderItemRequest
                    {
                        Discount = x.Discount,
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        UnitPrice = x.Price
                    }).ToArray()
                };


            }
            catch (Exception)
            {

                throw;
            }

            finally
            {

                IsBusy = false;
            }
        }


        [RelayCommand]

        private async Task GoBackPageAsync() {

            await Shell.Current.GoToAsync("..");
        }
    }
}

using BikeHub.Mobile.Pages;
using BikeHub.Shared.Dto.Response;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.Maui.Toolkit.Picker;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeHub.Mobile.ViewModel
{
    [QueryProperty(nameof(OrderId), "OrderId")]
    public partial class OrderViewModel : ObservableObject
    {
        [ObservableProperty]
        private string orderId;

        public string PageTitle => string.IsNullOrEmpty(OrderId)
                                      ? "Add Order"
                                      : "Edit Order - #" + OrderId;
        private void OnUserIdChanged(string value)
        {

            OnPropertyChanged(nameof(PageTitle));
        }

        public ObservableCollection<OrdersDto> Orders { get; set; }

        [ObservableProperty]
        private string selectedStatus;

        [ObservableProperty]
        private List<string> statusSource;

        public OrderViewModel()
        {

            StatusSource = new List<string> { "Processing", "Shipped", "Delivered", "Cancelled" };
            selectedStatus = StatusSource[0];
            Orders = new ObservableCollection<OrdersDto>();

            Orders.Add(new OrdersDto() { OrderId = 1, CustomerName = "John Doe", Status = "Delivered", TotalAmount = 250.75m, Image = "woman.png" });
            Orders.Add(new OrdersDto() { OrderId = 2, CustomerName = "Jane Smith", Status = "Processing", TotalAmount = 120.50m, Image = "man.png" });
            Orders.Add(new OrdersDto() { OrderId = 3, CustomerName = "Michael Johnson", Status = "Shipped", TotalAmount = 75.99m, Image = "woman.png" });
            Orders.Add(new OrdersDto() { OrderId = 4, CustomerName = "Emily Davis", Status = "Delivered", TotalAmount = 180.00m, Image = "man.png" });
            Orders.Add(new OrdersDto() { OrderId = 5, CustomerName = "William Brown", Status = "Cancelled", TotalAmount = 95.25m, Image = "woman.png" });
            Orders.Add(new OrdersDto() { OrderId = 6, CustomerName = "Olivia Wilson", Status = "Processing", TotalAmount = 210.40m, Image = "man.png" });
            Orders.Add(new OrdersDto() { OrderId = 7, CustomerName = "James Taylor", Status = "Shipped", TotalAmount = 300.00m, Image = "woman.png" });
            Orders.Add(new OrdersDto() { OrderId = 8, CustomerName = "Sophia Martinez", Status = "Delivered", TotalAmount = 50.75m, Image = "man.png" });
            Orders.Add(new OrdersDto() { OrderId = 9, CustomerName = "Benjamin Anderson", Status = "Processing", TotalAmount = 400.00m, Image = "woman.png" });
            Orders.Add(new OrdersDto() { OrderId = 10, CustomerName = "Ava Thomas", Status = "Delivered", TotalAmount = 150.20m, Image = "man.png" });


        }

        [RelayCommand]
        private void StatusChanged(object selectedItem)
        {
            if (selectedItem != null)
            {
                SelectedStatus = selectedItem.ToString();
            }

        }



        [RelayCommand]
        public async Task GotoNewOrder()
        {

            await Shell.Current.GoToAsync($"{nameof(AddEditOrders)}");

        }


        [RelayCommand]
        public async Task CreateNewOrderAsync()
        {
            await Shell.Current.GoToAsync($"{nameof(OrderPlacedPage)}");
        }

    }
}

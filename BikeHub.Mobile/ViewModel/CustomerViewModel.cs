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
    [QueryProperty(nameof(CustomerId), "customerId")]
    public partial class CustomerViewModel : ObservableObject
    {

        [ObservableProperty]
        private string customerId;

        [ObservableProperty]
        private CustomerDetailDto customerDetail;



        public string PageTitle => string.IsNullOrEmpty(customerId) ? "Add Customer" : "Edit Customer";

        partial void OnCustomerIdChanged(string value)
        {
            OnPropertyChanged(nameof(PageTitle));
        }

        public ObservableCollection<CustomersDto> Customers { get; set; }

        public CustomerViewModel()
        {
            Customers = new ObservableCollection<CustomersDto>();

            Customers.Add(new CustomersDto() { CustomerId = 1, CustomerName = "John Doe", Email = "john.doe@gmail.com", Phone = "123-456-7890", Image = "man.png" });
            Customers.Add(new CustomersDto() { CustomerId = 2, CustomerName = "Jane Smith", Email = "jane.smith@gmail.com", Phone = "987-654-3210", Image = "woman.png" });
            Customers.Add(new CustomersDto() { CustomerId = 3, CustomerName = "Michael Johnson", Email = "michael.johnson@gmail.com", Phone = "555-123-4567", Image = "man.png" });
            Customers.Add(new CustomersDto() { CustomerId = 4, CustomerName = "Emily Davis", Email = "emily.davis@gmail.com", Phone = "444-987-6543", Image = "woman.png" });
            Customers.Add(new CustomersDto() { CustomerId = 5, CustomerName = "William Brown", Email = "william.brown@gmail.com", Phone = "222-333-4444", Image = "man.png" });
            Customers.Add(new CustomersDto() { CustomerId = 6, CustomerName = "Olivia Wilson", Email = "olivia.wilson@gmail.com", Phone = "777-888-9999", Image = "woman.png" });
            Customers.Add(new CustomersDto() { CustomerId = 7, CustomerName = "James Taylor", Email = "james.taylor@gmail.com", Phone = "111-222-3333", Image = "man.png" });
            Customers.Add(new CustomersDto() { CustomerId = 8, CustomerName = "Sophia Martinez", Email = "sophia.martinez@gmail.com", Phone = "666-555-4444", Image = "woman.png" });
            Customers.Add(new CustomersDto() { CustomerId = 9, CustomerName = "Benjamin Anderson", Email = "ben.anderson@gmail.com", Phone = "888-777-6666", Image = "man.png" });
            Customers.Add(new CustomersDto() { CustomerId = 10, CustomerName = "Ava Thomas", Email = "ava.thomas@gmail.com", Phone = "999-000-1111", Image = "woman.png" });

            CustomerDetail = new CustomerDetailDto()
            {
                CustomerId = 1,
                CustomerName = "John Doe",
                Email = "john.doe@gmail.com",
                Phone = "123-456-7890",
                Street = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "12345",
                Image = "man.png"
            };
        }
        [RelayCommand]
        public async Task GotoAddCustomer()
        {

            await Shell.Current.GoToAsync($"{nameof(AddEditCustomer)}");

        }

        [RelayCommand]
        async Task GoToDetails(CustomersDto dto)
        {
            await Shell.Current.GoToAsync(nameof(CustomerDetailsPage), true, new Dictionary<string, object>
        {
            { "customerId", dto.CustomerId }
        });
        }
    }
}

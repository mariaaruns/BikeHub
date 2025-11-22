using BikeHub.Mobile.ApiServices;
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

    [QueryProperty(nameof(UserId), "UserId")]
    public partial class UserViewModel : ObservableObject
    {
     
        [ObservableProperty]
        private string userId;

        public string PageTitle =>string.IsNullOrEmpty(UserId)
          ? "Add User"
          : "Edit User";

        partial void OnUserIdChanged(string value) { 
        
            OnPropertyChanged(nameof(PageTitle));
        }

        public ObservableCollection<UsersDto> Users { get; set; }

        [ObservableProperty]
        private string selectedRole;

        [ObservableProperty]
        private List<string> rolestatus;

        public UserViewModel()
        {
            Users = new ObservableCollection<UsersDto>
        {
            new UsersDto { UserId = 1, FullName = "John Doe", Role = "Admin", Image = "man.png" ,IsActive=true},
            new UsersDto { UserId = 2, FullName = "Jane Smith", Role = "Manager", Image = "woman.png",IsActive=true },
            new UsersDto { UserId = 3, FullName = "Michael Johnson", Role = "Employee", Image = "man.png",IsActive=true },
            new UsersDto { UserId = 4, FullName = "Emily Davis", Role = "Employee", Image = "woman.png",IsActive=true },
            new UsersDto { UserId = 5, FullName = "William Brown", Role = "Manager", Image = "man.png" ,IsActive=false},
            new UsersDto { UserId = 6, FullName = "Olivia Wilson", Role = "Employee", Image = "woman.png",IsActive=false },
            new UsersDto { UserId = 7, FullName = "James Taylor", Role = "Admin", Image = "man.png" , IsActive = true},
            new UsersDto { UserId = 8, FullName = "Sophia Martinez", Role = "Employee", Image = "woman.png",IsActive=true },
            new UsersDto { UserId = 9, FullName = "Benjamin Anderson", Role = "Manager", Image = "man.png",IsActive=false },
            new UsersDto { UserId = 10, FullName = "Ava Thomas", Role = "Employee", Image = "woman.png",IsActive=true }
        };

            rolestatus = new List<string> { "ALL", "Admin", "Manager", "Employee" };
        }


        [RelayCommand]
        private void RoleChanged(object selectedItem) 
        {


            if (selectedItem != null)
            {
                SelectedRole= selectedItem.ToString();
            }

        }

        [RelayCommand]
        public async Task GotoAddUser()
        {

            await Shell.Current.GoToAsync(nameof(AddEditUsers));

        }

        [RelayCommand]
        public async Task RoleChipsChanged(object obj)
        {
            if (obj is string role)
            {
                SelectedRole = role;
                // Optional: Add your logic here (API call, filter, etc.)
                await Application.Current.MainPage.DisplayAlert("Role Changed", $"Selected: {SelectedRole}", "OK");
            }
        }

    }
}

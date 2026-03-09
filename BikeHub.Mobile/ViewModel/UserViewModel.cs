using Android.Webkit;
using BikeHub.Mobile.ApiServices;
using BikeHub.Mobile.Pages;
using BikeHub.Shared.Dto.Response;
using CommunityToolkit.Maui.Alerts;
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
        private readonly IUserApi _userApi;

        public UserViewModel(IUserApi userApi)
        {
            rolestatus = new List<string> { "ALL", "Admin", "User", "Mechanic" };
            selectedRole = "ALL";
            _userApi = userApi;
        }



        [ObservableProperty]
        private string userId;

        public string PageTitle => string.IsNullOrEmpty(UserId)
          ? "Add User"
          : "Edit User";

        partial void OnUserIdChanged(string value)
        {

            OnPropertyChanged(nameof(PageTitle));
        }

        [ObservableProperty]
        private ObservableCollection<UsersDto> _users = new();

        [ObservableProperty]
        private string selectedRole;


        private CancellationTokenSource _roleChangedCts;
        async partial void OnSelectedRoleChanged(string value)
        {
            if (value is not null)
            {
                _roleChangedCts?.Cancel();

                _roleChangedCts = new CancellationTokenSource();

                var token = _roleChangedCts.Token;

                await Task.Delay(500, token);

                Users.Clear();

                _ = LoadUsersAsync(token);

            }
        }

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private string _searchUser;

        [ObservableProperty]
        private bool _isLoadingMore;


        private CancellationTokenSource _searchUserCts;
        async partial void OnSearchUserChanged(string value)
        {

            try
            {
                if (value is not null)
                {
                    _searchUserCts?.Cancel();
                    _searchUserCts = new CancellationTokenSource();
                    var token = _searchUserCts.Token;
                    await Task.Delay(500, token);

                    Users.Clear();

                    _ = LoadUsersAsync(token);
                }
            }
            catch (TaskCanceledException)
            {
                //igonre
            }
            catch (Exception)
            {

                throw;
            }

     
        }

        [ObservableProperty]
        private List<string> rolestatus;


        [RelayCommand]
        public async Task GotoAddUserAsync()
        {

            await Shell.Current.GoToAsync(nameof(AddEditUsers));

        }


        private int _currentPage = 1;
        private int _pageSize = 20;
        [RelayCommand]
        public async Task LoadUsersAsync(CancellationToken cancellationToken)
        {

            try
            {
                if (IsLoadingMore) return;

                IsLoadingMore = true;

                var requestDto = new Shared.Dto.Request.UsersRequestDto
                {
                    SearchRole = SelectedRole == "ALL" ? string.Empty : SelectedRole,
                    SearchName = SearchUser ?? string.Empty,
                    PageNumber = _currentPage,
                    PageSize = _pageSize
                };

                var result = await _userApi.GetUsersAsync(requestDto, cancellationToken);
                if (result?.Data?.Data == null)
                {
                    return;
                }

                if (result != null && result.Status && result.Data != null)
                {
                    foreach (var user in result?.Data?.Data)
                    {
                        Users.Add(user);
                    }
                    _pageSize++;
                }

            }
            catch(OperationCanceledException)
            {
                // Ignore cancellation
            }
            catch (Exception ex)
            {
                await Toast.Make("Failed to Load users").Show();
            }
            finally
            {
                IsLoadingMore = false;
                IsRefreshing = false;
            }

        }

        [RelayCommand]
        public async Task RefreshUsersAsync()
        {
            try
            {
                IsRefreshing = true;
                _currentPage = 1;
                Users.Clear();
                await LoadUsersAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                await Toast.Make("Failed to refresh users").Show();
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }
}

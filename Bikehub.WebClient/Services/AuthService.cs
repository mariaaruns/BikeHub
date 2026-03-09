using BikeHub.Models;

namespace BikeHub.Services;

public class AuthService
{
    public AppUser? CurrentUser { get; private set; }
    public bool IsAuthenticated => CurrentUser != null;
    public event Action? OnAuthChanged;

    // Default permission sets per role
    public static UserPermissions DefaultPermissions(UserRole role) => role switch
    {
        UserRole.Admin => new UserPermissions
        {
            CanViewProducts = true, CanCreateProducts = true, CanEditProducts = true, CanDeleteProducts = true,
            CanViewCategories = true, CanCreateCategories = true, CanEditCategories = true, CanDeleteCategories = true,
            CanViewBrands = true, CanCreateBrands = true, CanEditBrands = true, CanDeleteBrands = true,
            CanViewCustomers = true, CanCreateCustomers = true, CanEditCustomers = true, CanDeleteCustomers = true,
            CanViewOrders = true, CanCreateOrders = true, CanUpdateOrderStatus = true, CanDeleteOrders = true,
            CanManageUsers = true,
            CanViewMechanicDashboard = true, CanViewTasks = true,
        },
        UserRole.Staff => new UserPermissions
        {
            CanViewProducts = true, CanViewCategories = true, CanViewBrands = true, CanViewCustomers = true,
            CanViewOrders = true, CanCreateOrders = true, CanUpdateOrderStatus = true,
        },
        UserRole.Mechanic => new UserPermissions
        {
            CanViewMechanicDashboard = true, CanViewTasks = true,
        },
        _ => new UserPermissions()
    };

    public Task<bool> LoginAsync(string email, string password)
    {
        var user = SeedData.Users.FirstOrDefault(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && u.Password == password);
        if (user == null) return Task.FromResult(false);
        CurrentUser = user;
        OnAuthChanged?.Invoke();
        return Task.FromResult(true);
    }

    public void Logout()
    {
        CurrentUser = null;
        OnAuthChanged?.Invoke();
    }

    public bool HasPermission(Func<UserPermissions, bool> check)
        => CurrentUser != null && check(CurrentUser.Permissions);
}

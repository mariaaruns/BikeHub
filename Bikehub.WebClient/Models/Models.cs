namespace BikeHub.Models;

// ── Enums ──────────────────────────────────────────────────────────────────────
public enum UserRole { Admin, Staff, Mechanic }
public enum OrderStatus { Pending, Confirmed, InService, ReadyForPickup, Completed, Cancelled }
public enum TaskStatusEnum { Assigned, InProgress, Completed }

// ── Auth ──────────────────────────────────────────────────────────────────────
public class AppUser
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";          // hashed in real app
    public UserRole Role { get; set; }
    public string Avatar { get; set; } = "";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public UserPermissions Permissions { get; set; } = new();
}

public class UserPermissions
{
    // Product
    public bool CanViewProducts   { get; set; } = true;
    public bool CanCreateProducts { get; set; }
    public bool CanEditProducts   { get; set; }
    public bool CanDeleteProducts { get; set; }
    // Category
    public bool CanViewCategories   { get; set; } = true;
    public bool CanCreateCategories { get; set; }
    public bool CanEditCategories   { get; set; }
    public bool CanDeleteCategories { get; set; }
    // Brand
    public bool CanViewBrands   { get; set; } = true;
    public bool CanCreateBrands { get; set; }
    public bool CanEditBrands   { get; set; }
    public bool CanDeleteBrands { get; set; }
    // Customer
    public bool CanViewCustomers   { get; set; } = true;
    public bool CanCreateCustomers { get; set; }
    public bool CanEditCustomers   { get; set; }
    public bool CanDeleteCustomers { get; set; }
    // Order
    public bool CanViewOrders    { get; set; } = true;
    public bool CanCreateOrders  { get; set; }
    public bool CanUpdateOrderStatus { get; set; }
    public bool CanDeleteOrders  { get; set; }
    // Users
    public bool CanManageUsers   { get; set; }
    // Mechanic
    public bool CanViewMechanicDashboard { get; set; }
    public bool CanViewTasks             { get; set; }
}

// ── Catalogue ─────────────────────────────────────────────────────────────────
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class Brand
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string LogoUrl { get; set; } = "";
    public string Country { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string ImageUrl { get; set; } = "";
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
    public int BrandId { get; set; }
    public string BrandName { get; set; } = "";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

// ── Customer ──────────────────────────────────────────────────────────────────
public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Address { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

// ── Order ─────────────────────────────────────────────────────────────────────
public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = "";
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = "";
    public List<OrderItem> Items { get; set; } = new();
    public decimal Total { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string Notes { get; set; } = "";
    public int? AssignedMechanicId { get; set; }
    public string? AssignedMechanicName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public class OrderItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal => Quantity * UnitPrice;
}

// ── Mechanic Task ─────────────────────────────────────────────────────────────
public class MechanicTask
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public string Description { get; set; } = "";
    public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Assigned;
    public string Priority { get; set; } = "Normal";   // Low / Normal / High / Urgent
    public int MechanicId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.Now;
    public DateTime? CompletedAt { get; set; }
}

// ── Dashboard Stats ───────────────────────────────────────────────────────────
public class DashboardStats
{
    public int TotalProducts  { get; set; }
    public int TotalCustomers { get; set; }
    public int TotalOrders    { get; set; }
    public decimal TotalRevenue { get; set; }
    public int PendingOrders   { get; set; }
    public int CompletedOrders { get; set; }
    public int ActiveMechanicTasks { get; set; }
}

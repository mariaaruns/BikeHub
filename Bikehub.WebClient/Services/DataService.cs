using BikeHub.Models;

namespace BikeHub.Services;

public class DataService
{
    // Live mutable lists (in real app: HTTP calls to your API)
    public List<Category> Categories { get; } = new(SeedData.Categories);
    public List<Brand>    Brands     { get; } = new(SeedData.Brands);
    public List<Product>  Products   { get; } = new(SeedData.Products);
    public List<Customer> Customers  { get; } = new(SeedData.Customers);
    public List<Order>    Orders     { get; } = new(SeedData.Orders);
    public List<MechanicTask> Tasks  { get; } = new(SeedData.Tasks);
    public List<AppUser>  Users      { get; } = new(SeedData.Users);

    private int _nextCatId    = 10;
    private int _nextBrandId  = 10;
    private int _nextProductId= 10;
    private int _nextCustId   = 10;
    private int _nextOrderId  = 1010;
    private int _nextTaskId   = 10;
    private int _nextUserId   = 10;

    // ── Categories ────────────────────────────────────────────────────────────
    public void AddCategory(Category c)    { c.Id = _nextCatId++; c.CreatedAt = DateTime.Now; Categories.Add(c); }
    public void UpdateCategory(Category c) { var i = Categories.FindIndex(x=>x.Id==c.Id); if(i>=0) Categories[i]=c; }
    public void DeleteCategory(int id)     { Categories.RemoveAll(x=>x.Id==id); }

    // ── Brands ────────────────────────────────────────────────────────────────
    public void AddBrand(Brand b)    { b.Id = _nextBrandId++; b.CreatedAt = DateTime.Now; Brands.Add(b); }
    public void UpdateBrand(Brand b) { var i = Brands.FindIndex(x=>x.Id==b.Id); if(i>=0) Brands[i]=b; }
    public void DeleteBrand(int id)  { Brands.RemoveAll(x=>x.Id==id); }

    // ── Products ──────────────────────────────────────────────────────────────
    public void AddProduct(Product p)    { p.Id = _nextProductId++; p.CreatedAt = DateTime.Now; Products.Add(p); }
    public void UpdateProduct(Product p) { var i = Products.FindIndex(x=>x.Id==p.Id); if(i>=0) Products[i]=p; }
    public void DeleteProduct(int id)    { Products.RemoveAll(x=>x.Id==id); }

    // ── Customers ─────────────────────────────────────────────────────────────
    public void AddCustomer(Customer c)    { c.Id = _nextCustId++; c.CreatedAt = DateTime.Now; Customers.Add(c); }
    public void UpdateCustomer(Customer c) { var i = Customers.FindIndex(x=>x.Id==c.Id); if(i>=0) Customers[i]=c; }
    public void DeleteCustomer(int id)     { Customers.RemoveAll(x=>x.Id==id); }

    // ── Orders ────────────────────────────────────────────────────────────────
    public void AddOrder(Order o)
    {
        o.Id = _nextOrderId;
        o.OrderNumber = $"ORD-{_nextOrderId++}";
        o.CreatedAt = DateTime.Now;
        o.UpdatedAt = DateTime.Now;
        o.Total = o.Items.Sum(i => i.Subtotal);
        Orders.Add(o);

        // Auto-create mechanic task if mechanic assigned
        if (o.AssignedMechanicId.HasValue)
        {
            var task = new MechanicTask
            {
                Id = _nextTaskId++,
                OrderId = o.Id,
                OrderNumber = o.OrderNumber,
                CustomerName = o.CustomerName,
                Description = $"Service for order {o.OrderNumber}",
                Status = Models.TaskStatusEnum.Assigned,
                Priority = "Normal",
                MechanicId = o.AssignedMechanicId.Value,
                AssignedAt = DateTime.Now,
            };
            Tasks.Add(task);
        }
    }

    public void UpdateOrderStatus(int id, OrderStatus status)
    {
        var o = Orders.FirstOrDefault(x=>x.Id==id);
        if (o!=null) { o.Status = status; o.UpdatedAt = DateTime.Now; }
    }
    public void DeleteOrder(int id) { Orders.RemoveAll(x=>x.Id==id); }

    // ── Tasks ─────────────────────────────────────────────────────────────────
    public void UpdateTaskStatus(int id, Models.TaskStatusEnum status)
    {
        var t = Tasks.FirstOrDefault(x=>x.Id==id);
        if (t!=null) { t.Status = status; if(status==Models.TaskStatusEnum.Completed) t.CompletedAt=DateTime.Now; }
    }

    // ── Users ─────────────────────────────────────────────────────────────────
    public void AddUser(AppUser u)    { u.Id = _nextUserId++; u.CreatedAt = DateTime.Now; Users.Add(u); }
    public void UpdateUser(AppUser u) { var i = Users.FindIndex(x=>x.Id==u.Id); if(i>=0) Users[i]=u; }
    public void DeleteUser(int id)    { Users.RemoveAll(x=>x.Id==id); }

    // ── Dashboard Stats ───────────────────────────────────────────────────────
    public DashboardStats GetStats() => new()
    {
        TotalProducts   = Products.Count,
        TotalCustomers  = Customers.Count,
        TotalOrders     = Orders.Count,
        TotalRevenue    = Orders.Where(o=>o.Status==OrderStatus.Completed).Sum(o=>o.Total),
        PendingOrders   = Orders.Count(o=>o.Status==OrderStatus.Pending),
        CompletedOrders = Orders.Count(o=>o.Status==OrderStatus.Completed),
        ActiveMechanicTasks = Tasks.Count(t=>t.Status!=Models.TaskStatusEnum.Completed),
    };

    public List<(string Month, decimal Revenue)> GetMonthlyRevenue()
    {
        var months = Enumerable.Range(1,6).Select(i => DateTime.Now.AddMonths(-6+i)).ToList();
        return months.Select(m => (
            m.ToString("MMM"),
            Orders.Where(o => o.CreatedAt.Month == m.Month && o.Status == OrderStatus.Completed).Sum(o => o.Total) +
            (decimal)(new Random(m.Month).NextDouble() * 3000 + 1000) // fill empty months
        )).ToList();
    }
}

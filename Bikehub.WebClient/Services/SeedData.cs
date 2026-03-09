using BikeHub.Models;

namespace BikeHub.Services;

public static class SeedData
{
    // ── Users ──────────────────────────────────────────────────────────────────
    public static List<AppUser> Users { get; } = new()
    {
        new AppUser { Id=1, Name="Alex Morgan", Email="admin@bikehub.com", Password="admin123",
            Role=UserRole.Admin, Avatar="AM", IsActive=true,
            Permissions=AuthService.DefaultPermissions(UserRole.Admin) },
        new AppUser { Id=2, Name="Jordan Lee", Email="staff@bikehub.com", Password="staff123",
            Role=UserRole.Staff, Avatar="JL", IsActive=true,
            Permissions=AuthService.DefaultPermissions(UserRole.Staff) },
        new AppUser { Id=3, Name="Casey Rivera", Email="mechanic@bikehub.com", Password="mech123",
            Role=UserRole.Mechanic, Avatar="CR", IsActive=true,
            Permissions=AuthService.DefaultPermissions(UserRole.Mechanic) },
        new AppUser { Id=4, Name="Sam Patel", Email="sam@bikehub.com", Password="staff123",
            Role=UserRole.Staff, Avatar="SP", IsActive=true,
            Permissions=AuthService.DefaultPermissions(UserRole.Staff) },
    };

    // ── Categories ────────────────────────────────────────────────────────────
    public static List<Category> Categories { get; } = new()
    {
        new Category { Id=1, Name="Mountain Bikes",   Description="Off-road & trail bikes",    ImageUrl="🏔️", CreatedAt=DateTime.Now.AddDays(-90) },
        new Category { Id=2, Name="Road Bikes",       Description="Speed & endurance bikes",   ImageUrl="🛣️", CreatedAt=DateTime.Now.AddDays(-85) },
        new Category { Id=3, Name="Electric Bikes",   Description="E-assisted bikes",          ImageUrl="⚡", CreatedAt=DateTime.Now.AddDays(-70) },
        new Category { Id=4, Name="BMX",              Description="Trick & stunt bikes",       ImageUrl="🎯", CreatedAt=DateTime.Now.AddDays(-60) },
        new Category { Id=5, Name="Accessories",      Description="Helmets, locks, lights",    ImageUrl="🔧", CreatedAt=DateTime.Now.AddDays(-50) },
        new Category { Id=6, Name="Kids Bikes",       Description="Bikes for children",        ImageUrl="🌟", CreatedAt=DateTime.Now.AddDays(-40) },
    };

    // ── Brands ────────────────────────────────────────────────────────────────
    public static List<Brand> Brands { get; } = new()
    {
        new Brand { Id=1, Name="Trek",      LogoUrl="🚲", Country="USA",        CreatedAt=DateTime.Now.AddDays(-100) },
        new Brand { Id=2, Name="Specialized", LogoUrl="⭐", Country="USA",      CreatedAt=DateTime.Now.AddDays(-98) },
        new Brand { Id=3, Name="Giant",     LogoUrl="🏆", Country="Taiwan",     CreatedAt=DateTime.Now.AddDays(-95) },
        new Brand { Id=4, Name="Cannondale",LogoUrl="💎", Country="USA",        CreatedAt=DateTime.Now.AddDays(-90) },
        new Brand { Id=5, Name="Scott",     LogoUrl="🔵", Country="Switzerland",CreatedAt=DateTime.Now.AddDays(-85) },
        new Brand { Id=6, Name="Merida",    LogoUrl="🌀", Country="Taiwan",     CreatedAt=DateTime.Now.AddDays(-80) },
    };

    // ── Products ──────────────────────────────────────────────────────────────
    public static List<Product> Products { get; } = new()
    {
        new Product { Id=1, Name="Trek Marlin 7", Price=899, Stock=12, CategoryId=1, CategoryName="Mountain Bikes", BrandId=1, BrandName="Trek",     ImageUrl="🚵", Description="29-inch hardtail MTB", CreatedAt=DateTime.Now.AddDays(-80) },
        new Product { Id=2, Name="Specialized Allez", Price=1099, Stock=7, CategoryId=2, CategoryName="Road Bikes",      BrandId=2, BrandName="Specialized", ImageUrl="🚴", Description="Entry road racer", CreatedAt=DateTime.Now.AddDays(-70) },
        new Product { Id=3, Name="Giant Talon 2", Price=749, Stock=15, CategoryId=1, CategoryName="Mountain Bikes", BrandId=3, BrandName="Giant",    ImageUrl="🏔", Description="Versatile trail bike",  CreatedAt=DateTime.Now.AddDays(-65) },
        new Product { Id=4, Name="Trek Powerfly 5", Price=3499, Stock=4, CategoryId=3, CategoryName="Electric Bikes",  BrandId=1, BrandName="Trek",     ImageUrl="⚡", Description="Full-sus e-MTB",     CreatedAt=DateTime.Now.AddDays(-60) },
        new Product { Id=5, Name="Cannondale Habit", Price=2299, Stock=6, CategoryId=1, CategoryName="Mountain Bikes", BrandId=4, BrandName="Cannondale",ImageUrl="🏕", Description="Full-suspension 29er",CreatedAt=DateTime.Now.AddDays(-55) },
        new Product { Id=6, Name="Scott Speedster 40", Price=1299, Stock=9, CategoryId=2, CategoryName="Road Bikes",      BrandId=5, BrandName="Scott",    ImageUrl="💨", Description="Carbon fork road bike",CreatedAt=DateTime.Now.AddDays(-50) },
        new Product { Id=7, Name="Bell Helmet Pro", Price=129, Stock=28, CategoryId=5, CategoryName="Accessories",    BrandId=1, BrandName="Trek",     ImageUrl="⛑", Description="MIPS road helmet",   CreatedAt=DateTime.Now.AddDays(-45) },
        new Product { Id=8, Name="Kryptonite Lock", Price=79, Stock=40, CategoryId=5, CategoryName="Accessories",    BrandId=3, BrandName="Giant",    ImageUrl="🔒", Description="U-lock heavy duty",  CreatedAt=DateTime.Now.AddDays(-40) },
    };

    // ── Customers ─────────────────────────────────────────────────────────────
    public static List<Customer> Customers { get; } = new()
    {
        new Customer { Id=1, Name="Sarah Chen",    Email="sarah@email.com",   Phone="555-0101", Address="42 Elm St, Portland OR",       ImageUrl="SC", CreatedAt=DateTime.Now.AddDays(-120) },
        new Customer { Id=2, Name="Marcus Webb",   Email="marcus@email.com",  Phone="555-0102", Address="17 Oak Ave, Seattle WA",        ImageUrl="MW", CreatedAt=DateTime.Now.AddDays(-100) },
        new Customer { Id=3, Name="Priya Nair",    Email="priya@email.com",   Phone="555-0103", Address="88 Pine Rd, Denver CO",         ImageUrl="PN", CreatedAt=DateTime.Now.AddDays(-90) },
        new Customer { Id=4, Name="Tom Eriksson",  Email="tom@email.com",     Phone="555-0104", Address="5 Cedar Ln, Austin TX",         ImageUrl="TE", CreatedAt=DateTime.Now.AddDays(-75) },
        new Customer { Id=5, Name="Lily Santos",   Email="lily@email.com",    Phone="555-0105", Address="200 Birch Blvd, Chicago IL",    ImageUrl="LS", CreatedAt=DateTime.Now.AddDays(-60) },
        new Customer { Id=6, Name="Omar Hassan",   Email="omar@email.com",    Phone="555-0106", Address="33 Maple Dr, Phoenix AZ",       ImageUrl="OH", CreatedAt=DateTime.Now.AddDays(-45) },
        new Customer { Id=7, Name="Nina Volkova",  Email="nina@email.com",    Phone="555-0107", Address="99 Spruce Way, Boston MA",      ImageUrl="NV", CreatedAt=DateTime.Now.AddDays(-30) },
    };

    // ── Orders ────────────────────────────────────────────────────────────────
    public static List<Order> Orders { get; } = new()
    {
        new Order { Id=1, OrderNumber="ORD-1001", CustomerId=1, CustomerName="Sarah Chen",   Status=OrderStatus.Completed,      Total=1028, AssignedMechanicId=3, AssignedMechanicName="Casey Rivera", CreatedAt=DateTime.Now.AddDays(-30), Items=new(){ new(){ ProductId=1, ProductName="Trek Marlin 7",    Quantity=1, UnitPrice=899 }, new(){ ProductId=7, ProductName="Bell Helmet Pro", Quantity=1, UnitPrice=129 } } },
        new Order { Id=2, OrderNumber="ORD-1002", CustomerId=2, CustomerName="Marcus Webb",  Status=OrderStatus.InService,      Total=3499, AssignedMechanicId=3, AssignedMechanicName="Casey Rivera", CreatedAt=DateTime.Now.AddDays(-10), Items=new(){ new(){ ProductId=4, ProductName="Trek Powerfly 5", Quantity=1, UnitPrice=3499 } } },
        new Order { Id=3, OrderNumber="ORD-1003", CustomerId=3, CustomerName="Priya Nair",   Status=OrderStatus.Pending,        Total=749,  CreatedAt=DateTime.Now.AddDays(-5),  Items=new(){ new(){ ProductId=3, ProductName="Giant Talon 2",   Quantity=1, UnitPrice=749 } } },
        new Order { Id=4, OrderNumber="ORD-1004", CustomerId=4, CustomerName="Tom Eriksson", Status=OrderStatus.Confirmed,      Total=2378, AssignedMechanicId=3, AssignedMechanicName="Casey Rivera", CreatedAt=DateTime.Now.AddDays(-3),  Items=new(){ new(){ ProductId=5, ProductName="Cannondale Habit", Quantity=1, UnitPrice=2299 }, new(){ ProductId=8, ProductName="Kryptonite Lock", Quantity=1, UnitPrice=79 } } },
        new Order { Id=5, OrderNumber="ORD-1005", CustomerId=5, CustomerName="Lily Santos",  Status=OrderStatus.ReadyForPickup, Total=1099, CreatedAt=DateTime.Now.AddDays(-2),  Items=new(){ new(){ ProductId=2, ProductName="Specialized Allez", Quantity=1, UnitPrice=1099 } } },
        new Order { Id=6, OrderNumber="ORD-1006", CustomerId=6, CustomerName="Omar Hassan",  Status=OrderStatus.Pending,        Total=1428, CreatedAt=DateTime.Now.AddDays(-1),  Items=new(){ new(){ ProductId=6, ProductName="Scott Speedster 40", Quantity=1, UnitPrice=1299 }, new(){ ProductId=8, ProductName="Kryptonite Lock", Quantity=1, UnitPrice=79 }, new(){ ProductId=7, ProductName="Bell Helmet Pro", Quantity=1, UnitPrice=129 } } },
    };

    // ── Mechanic Tasks ────────────────────────────────────────────────────────
    public static List<MechanicTask> Tasks { get; } = new()
    {
        new MechanicTask { Id=1, OrderId=1, OrderNumber="ORD-1001", CustomerName="Sarah Chen",   Description="Full bike assembly & brake adjustment", Status=Models.TaskStatusEnum.Completed, Priority="Normal", MechanicId=3, AssignedAt=DateTime.Now.AddDays(-30), CompletedAt=DateTime.Now.AddDays(-28) },
        new MechanicTask { Id=2, OrderId=2, OrderNumber="ORD-1002", CustomerName="Marcus Webb",  Description="E-bike setup & firmware update",         Status=Models.TaskStatusEnum.InProgress, Priority="High",   MechanicId=3, AssignedAt=DateTime.Now.AddDays(-10) },
        new MechanicTask { Id=3, OrderId=4, OrderNumber="ORD-1004", CustomerName="Tom Eriksson", Description="Full-suspension tune & wheel true",       Status=Models.TaskStatusEnum.Assigned, Priority="Urgent", MechanicId=3, AssignedAt=DateTime.Now.AddDays(-3) },
    };
}

namespace BikeHub.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; } 
        public string UserName { get; set; } = string.Empty;
        public string NormalizedUserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NormalizedEmail { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public string SecurityStamp { get; set; } = Guid.NewGuid().ToString();
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginDate { get; set; }
    }

    public class ApplicationRole
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    }

    public class ApplicationUserRole
    {
        public int UserId { get; set; } 
        public int RoleId { get; set; } 
    }
}

using BikeHub.Models;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;

namespace BikeHub.Service
{
    public class UserStore : IUserStore<ApplicationUser>,
        IUserPasswordStore<ApplicationUser>,
        IUserEmailStore<ApplicationUser>,
        IUserRoleStore<ApplicationUser>,
        IUserClaimStore<ApplicationUser>,
        IUserSecurityStampStore<ApplicationUser>,
        IUserLockoutStore<ApplicationUser>
    {
        private readonly IDbConnection _connection;

        public UserStore(IDbConnection connection)
        {
            this._connection = connection;
        }
        public async Task AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            const string sql = "INSERT INTO UserClaims (UserId, ClaimType, ClaimValue) VALUES (@UserId, @ClaimType, @ClaimValue)";
            var claimData = claims.Select(c => new { UserId = user.Id, ClaimType = c.Type, ClaimValue = c.Value });
            await _connection.ExecuteAsync(sql, claimData);
        }

        public async  Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            const string roleQuery = "SELECT Id FROM Roles WHERE NormalizedName = @roleName";
            var roleId = await _connection.QuerySingleOrDefaultAsync<int>(roleQuery, new { roleName });

            if (roleId != 0)
            {
                const string sql = "INSERT INTO UserRoles (UserId, RoleId) VALUES (@userId, @roleId)";
                await _connection.ExecuteAsync(sql, new { userId = user.Id, roleId });
            }
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var validationErrors = new List<IdentityError>();
            if (string.IsNullOrWhiteSpace(user.UserName))
                validationErrors.Add(new IdentityError { Code = "InvalidUserName", Description = "UserName is required." });
            if (string.IsNullOrWhiteSpace(user.Email))
                validationErrors.Add(new IdentityError { Code = "InvalidEmail", Description = "Email is required." });

            if (validationErrors.Count > 0)
                return IdentityResult.Failed(validationErrors.ToArray());

            const string sql = @"
                INSERT INTO Users ( UserName, NormalizedUserName, Email, NormalizedEmail, 
                    EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, 
                    PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, 
                    AccessFailedCount, CreatedDate,FirstName,LastName,Image)
                VALUES ( @UserName, @NormalizedUserName, @Email, @NormalizedEmail, 
                    @EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @PhoneNumber, 
                    @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, 
                    @AccessFailedCount, @CreatedDate,@FirstName,@LastName,@Image)";

            try
            {
                var affected = await _connection.ExecuteAsync(sql, user);
                if (affected > 0)
                    return IdentityResult.Success;

                return IdentityResult.Failed(new IdentityError
                {
                    Code = "InsertFailed",
                    Description = "User insert affected 0 rows."
                });
            }
            catch(Exception ex) {
                var msg = ex.Message ?? "Unknown database error";

                if (msg.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase) ||
                    msg.Contains("duplicate", StringComparison.OrdinalIgnoreCase) ||
                    msg.Contains("Violation of UNIQUE KEY", StringComparison.OrdinalIgnoreCase) ||
                    msg.Contains("duplicate key", StringComparison.OrdinalIgnoreCase))
                {
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "DuplicateUser",
                        Description = "A user with the same username or email already exists."
                    });
                }
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DatabaseError",
                    Description = msg
                });
            }
            
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            const string sql = "DELETE FROM Users WHERE Id = @Id";
            var result = await _connection.ExecuteAsync(sql, new { user.Id });
            return result > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }

        public void Dispose()
        {
            
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            const string sql = "SELECT * FROM Users WHERE NormalizedEmail = @normalizedEmail";
            return await _connection.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { normalizedEmail });
        }

        public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            const string sql = "SELECT * FROM Users WHERE Id = @userId";
            return await _connection.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { userId });

        }

        public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            const string sql = "SELECT * FROM Users WHERE NormalizedUserName = @normalizedUserName";
            return await _connection.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { normalizedUserName });
        }

        public Task<int> GetAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            const string sql = "SELECT ClaimType, ClaimValue FROM UserClaims WHERE UserId = @userId";
            var claims = await _connection.QueryAsync(sql, new { userId = user.Id });
            return claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
        }

        public Task<string?> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
             return Task.FromResult(user.LockoutEnd);
        }

        public Task<string?> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
              
        
            return Task.FromResult(user.PasswordHash);
        
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT r.Name FROM Roles r
                INNER JOIN UserRoles ur ON r.Id = ur.RoleId
                WHERE ur.UserId = @userId";

            var roles = await _connection.QueryAsync<string>(sql, new { userId = user.Id });
            return roles.ToList();
        }

        public Task<string?> GetSecurityStampAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public async Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT top 1 Id FROM Users u where [UserName] =@username";

            var userId = await _connection.QueryFirstOrDefaultAsync<ApplicationUser>(sql, new { @username = user.UserName });

            if (userId is not null)
            {
                return userId.Id.ToString();
            }
            return null;
        }

        public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public async Task<IList<ApplicationUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT u.* FROM Users u
                INNER JOIN UserClaims uc ON u.Id = uc.UserId
                WHERE uc.ClaimType = @ClaimType AND uc.ClaimValue = @ClaimValue";

            var users = await _connection.QueryAsync<ApplicationUser>(sql, new { ClaimType = claim.Type, ClaimValue = claim.Value });
            return users.ToList();
        }

        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT u.* FROM Users u
                INNER JOIN UserRoles ur ON u.Id = ur.UserId
                INNER JOIN Roles r ON ur.RoleId = r.Id
                WHERE r.NormalizedName = @roleName";

            var users = await _connection.QueryAsync<ApplicationUser>(sql, new { roleName });
            return users.ToList();
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public  Task<int> IncrementAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT COUNT(1) FROM UserRoles ur
                INNER JOIN Roles r ON ur.RoleId = r.Id
                WHERE ur.UserId = @userId AND r.NormalizedName = @roleName";

            var count = await _connection.QuerySingleAsync<int>(sql, new { userId = user.Id, roleName });
            return count > 0;
        }

        public async Task RemoveClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            const string sql = "DELETE FROM UserClaims WHERE UserId = @UserId AND ClaimType = @ClaimType AND ClaimValue = @ClaimValue";
            var claimData = claims.Select(c => new { UserId = user.Id, ClaimType = c.Type, ClaimValue = c.Value });
            await _connection.ExecuteAsync(sql, claimData);

        }

        public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            const string sql = @"
                DELETE ur FROM UserRoles ur
                INNER JOIN Roles r ON ur.RoleId = r.Id
                WHERE ur.UserId = @userId AND r.NormalizedName = @roleName";

            await _connection.ExecuteAsync(sql, new { userId = user.Id, roleName });
        }

        public async Task ReplaceClaimAsync(ApplicationUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            const string sql = @"
                UPDATE UserClaims 
                SET ClaimType = @NewClaimType, ClaimValue = @NewClaimValue
                WHERE UserId = @UserId AND ClaimType = @ClaimType AND ClaimValue = @ClaimValue";

            await _connection.ExecuteAsync(sql, new
            {
                UserId = user.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                NewClaimType = newClaim.Type,
                NewClaimValue = newClaim.Value
            });
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount = 0;
            return Task.CompletedTask;
        }

        public Task SetEmailAsync(ApplicationUser user, string? email, CancellationToken cancellationToken)
        {
            user.Email = email ?? string.Empty;
            return Task.CompletedTask;
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;
            return Task.CompletedTask;
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEnd = lockoutEnd;
            return Task.CompletedTask;
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail ?? string.Empty;
            return Task.CompletedTask;
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName ?? string.Empty;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash ?? string.Empty;
            return Task.CompletedTask;
        }

        public Task SetSecurityStampAsync(ApplicationUser user, string stamp, CancellationToken cancellationToken)
        {
            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
        {
            user.UserName = userName ?? string.Empty;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            const string sql = @"
                UPDATE Users SET 
                    UserName = @UserName, NormalizedUserName = @NormalizedUserName,
                    Email = @Email, NormalizedEmail = @NormalizedEmail,
                    EmailConfirmed = @EmailConfirmed, PasswordHash = @PasswordHash,
                    SecurityStamp = @SecurityStamp, ConcurrencyStamp = @ConcurrencyStamp,
                    PhoneNumber = @PhoneNumber, PhoneNumberConfirmed = @PhoneNumberConfirmed,
                    TwoFactorEnabled = @TwoFactorEnabled, LockoutEnd = @LockoutEnd,
                    LockoutEnabled = @LockoutEnabled, AccessFailedCount = @AccessFailedCount,
                    LastLoginDate = @LastLoginDate
                      WHERE Id = @Id";

            var result = await _connection.ExecuteAsync(sql, user);
            return result > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }
    }
}

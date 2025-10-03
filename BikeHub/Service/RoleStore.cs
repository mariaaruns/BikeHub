using BikeHub.Models;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;

namespace BikeHub.Service
{
    public class RoleStore : IRoleStore<ApplicationRole>, IRoleClaimStore<ApplicationRole>
    {
        private readonly IDbConnection _connection;

        public RoleStore(IDbConnection connection)
        {
            _connection = connection;
        }

        public  async Task AddClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            const string sql = "INSERT INTO AspNetRoleClaims (ClaimType, ClaimValue) VALUES (@RoleId, @ClaimType, @ClaimValue)";
            await _connection.ExecuteAsync(sql, new {ClaimType = claim.Type, ClaimValue = claim.Value });
        }

        public async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO AspNetRoles (Name, NormalizedName, ConcurrencyStamp)
                VALUES (@Name, @NormalizedName, @ConcurrencyStamp)";

            var result = await _connection.ExecuteAsync(sql, role);
            return result > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            const string sql = "DELETE FROM AspNetRoles WHERE Id = @Id";
            var result = await _connection.ExecuteAsync(sql, new { role.Id });
            return result > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            const string sql = "SELECT * FROM AspNetRoles WHERE Id = @roleId";
            return await _connection.QuerySingleOrDefaultAsync<ApplicationRole>(sql, new { roleId });
        }

        public async Task<ApplicationRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            const string sql = "SELECT * FROM AspNetRoles WHERE NormalizedName = @normalizedRoleName";
            return await _connection.QuerySingleOrDefaultAsync<ApplicationRole>(sql, new { normalizedRoleName });
        }

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationRole role, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT ClaimType, ClaimValue FROM AspNetRoleClaims WHERE RoleId = @roleId";
            var claims = await _connection.QueryAsync(sql, new { roleId = role.Id });
            return claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
        }

        public Task<string?> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);    
        }

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string?> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public async Task RemoveClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            const string sql = "DELETE FROM AspNetRoleClaims WHERE RoleId = @RoleId AND ClaimType = @ClaimType AND ClaimValue = @ClaimValue";
            await _connection.ExecuteAsync(sql, new { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value });
        }
        

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string? normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName ?? string.Empty;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(ApplicationRole role, string? roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName ?? string.Empty;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            const string sql = @"
                UPDATE AspNetRoles SET 
                    Name = @Name, NormalizedName = @NormalizedName, ConcurrencyStamp = @ConcurrencyStamp
                WHERE Id = @Id";

            var result = await _connection.ExecuteAsync(sql, role);
            return result > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }
    }
}

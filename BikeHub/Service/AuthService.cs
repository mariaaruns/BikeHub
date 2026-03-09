using BikeHub.Repository.IRepository;
using BikeHub.Service.Interface;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Request.AuthDto;
using BikeHub.Shared.Dto.Response;
using BikeHub.Shared.Dto.Response.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BikeHub.Service
{
    public class AuthService : IAuthService
    {
        private readonly IMemoryCache _cache;
        private readonly IUserRepository _userRepostitory;

        private readonly IConfiguration _configuration;
        public AuthService(IUserRepository userRepostitory, IConfiguration configuration, IMemoryCache cache)
        {
            this._userRepostitory = userRepostitory;
            _configuration = configuration;
            _cache = cache;
        }
        public Task<dynamic> AddNewPolicy()
        {
            throw new NotImplementedException();
        }

        public Task<dynamic> AddOrEditMenu(long? menuId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddOrEditPolicyForUser(long userId, ApplyPolicyDto[] dto)
        {
            try
            {
                var isRowsAffected = await _userRepostitory.AddOrRemovePolicyForUser(userId, dto);

                if (isRowsAffected)
                {
                    var cacheKey = $"POLICIES_{userId}";

                    if (_cache.TryGetValue(cacheKey, out HashSet<string> policies))
                    {
                        _cache.Remove(cacheKey);
                    }
                }

                return isRowsAffected;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<bool> CreateUser(CreateUserDto dto)
        {
            try
            {
                var PasswordHash = new PasswordHasher<CreateUserDto>()
                                    .HashPassword(dto, dto.Password);
                dto.Password = PasswordHash;
                var result = _userRepostitory.CreateUser(dto);
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<JwtResponse> GenerateJwtTokenAndPolicyAsync(LoginDto dto)
        {
            try
            {

                if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
                {
                    throw new Exception("Username and password cannot be empty");
                }



                var user = await _userRepostitory.GetUserInfo(dto);

                if (user == null)
                {
                    throw new Exception("Invalid username or password");
                }

                var _passwordHasher = new PasswordHasher<LoginDto>();


                var passwordHashed = _passwordHasher.VerifyHashedPassword(dto, user.Password, dto.Password);

                if (passwordHashed == PasswordVerificationResult.Failed)
                {
                    throw new Exception("Invalid username or password");
                }

                //List<UserPolicyResponse> getUserPolicy = await GetAllPoliciesByUserId(user.UserId);
                //string[] policyCodes= getUserPolicy
                //     .Where(x=>x.Haspermission==true)                    
                //    .Select(p => p.Code).ToArray();

                //var policyCodes = await GetPoliciesAsync(user.UserId);

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.UserName ?? string.Empty),
                    new Claim("name", $"{user.FirstName} {user.LastName}".Trim())
                };

                claims.Add(new Claim(ClaimTypes.Role, user.RoleName));

                // Read JWT settings from configuration
                var keySecret = _configuration["Jwt:Key"];
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var expiresMinutes = int.TryParse(_configuration["Jwt:ExpiresMinutes"], out var m) ? m : 120;


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keySecret));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var expires = DateTime.UtcNow.AddMinutes(expiresMinutes);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: expires,
                    signingCredentials: creds);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                var payload = new JwtResponse
                {
                    Token = tokenString,
                    Expires = expires,
                    Email = user.UserName,
                    Name = $"{user.FirstName} {user.LastName}".Trim(),
                    ProfileImage = $"/{commonInfo.USER_IMG_PATH}/{user.Image}",
                };


                return payload;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UserPolicyResponse>> GetAllPoliciesByUserId(long userId)
        {
            try
            {
                var policies = await _userRepostitory.GetUserPoliciesByUserId(userId);
                return policies;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<PagedResult<UsersDto>> GetAllUser(UsersRequestDto dto)
        {
            try
            {
                var result = await _userRepostitory.GetAllUser(dto);
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<HashSet<string>> GetPoliciesAsync(long userId)
        {

            var cacheKey = $"POLICIES_{userId}";

            if (_cache.TryGetValue(cacheKey, out HashSet<string> policies))
            {
                return policies; // ✅ FAST PATH
            }

            //// 🔄 CACHE MISS → FALLBACK TO DB
            policies = (await GetAllPoliciesByUserId(userId))
                                .Where(x => x.Haspermission == true)
                                .Select(p => p.Code).ToHashSet();

            //// ❗ Even empty set should be cached
            _cache.Set(
                cacheKey,
                policies,
                TimeSpan.FromHours(2)
            );

            return policies;
        }

        public Task<UserByIdDto> GetUserById(long userId)
        {
            try
            {
                return _userRepostitory.GetUserById(userId);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public Task<UsersDto> GetUserByUserName(string userName)
        {
            try
            {
                var result = _userRepostitory.GetUserByUserName(userName);

                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public Task<dynamic> GetUserMenuByUserId(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<dynamic> RemoveMenu()
        {
            throw new NotImplementedException();
        }

        public Task<dynamic> RemovePolicy(long policyId)
        {
            throw new NotImplementedException();
        }


        public async Task<RolesDto[]> GetAllRoles()
        {
            try
            {
                var result = await _userRepostitory.GetRolesAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> UpdateUser(UpdateUserDto dto)
        {
            try
            {
                return await _userRepostitory.UpdateUser(dto);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

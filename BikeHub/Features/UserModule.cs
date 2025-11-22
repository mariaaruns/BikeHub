using BikeHub.Models;
using BikeHub.Service;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Response;
using Carter;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BikeHub.Features
{
    public class UserModule : ICarterModule
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly RoleManager<ApplicationRole> _roleManager;


        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/createUser", async ([FromForm] RegisterDto dto, [FromServices] UserManager<ApplicationUser> userManager) =>
            {

                if (dto.ImgFile?.Length > 0)
                {


                }

                var user = new ApplicationUser
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    UserName = dto.Email,
                    NormalizedUserName = dto.Email.ToUpper(),
                    Email = dto.Email,
                    NormalizedEmail = dto.Email.ToUpper(),
                    Image = dto.Image,
                };
                var result = await userManager.CreateAsync(user, dto.Password);

                if (result.Succeeded)
                {

                    Results.Ok(ApiResponse<string>.Success("User Register Successfully"));
                }
                else
                {

                    var errors = result.Errors.Select(e => e.Description);

                    Results.BadRequest(ApiResponse<string>.Fail("User Register failed", errors: errors));
                }

            }).WithTags("Users").DisableAntiforgery();


            app.MapPost("/login", async ([FromBody] LoginDto dto,
                                        [FromServices] UserManager<ApplicationUser> userManager,
                                        [FromServices] IConfiguration config) =>
            {
                if (dto is null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                {
                    return Results.BadRequest(ApiResponse<string>.Fail("Invalid request", errors: new[] { "Email and Password are required." }));
                }

                var user = await userManager.FindByEmailAsync(dto.Email);
                if (user is null)
                {
                    return Results.BadRequest(ApiResponse<string>.Fail("Invalid credentials", errors: new[] { "Invalid email or password." }));
                }

                var passwordValid = await userManager.CheckPasswordAsync(user, dto.Password);
                if (!passwordValid)
                {
                    return Results.BadRequest(ApiResponse<string>.Fail("Invalid credentials", errors: new[] { "Invalid email or password." }));
                }

                // Build claims
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                    new Claim("name", $"{user.FirstName} {user.LastName}".Trim())
                };

                var roles = await userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                // Read JWT settings from configuration
                var keySecret = config["Jwt:Key"];
                var issuer = config["Jwt:Issuer"];
                var audience = config["Jwt:Audience"];
                var expiresMinutes = int.TryParse(config["Jwt:ExpiresMinutes"], out var m) ? m : 60;

                if (string.IsNullOrWhiteSpace(keySecret))
                {
                    return Results.Problem("JWT configuration is missing (Jwt:Key).");
                }

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
                    Email = user.Email,
                    Name = $"{user.FirstName} {user.LastName}".Trim()
                };

                return Results.Ok(ApiResponse<JwtResponse>.Success(payload));

            }).WithTags("Users").DisableAntiforgery();
        }
    }

}

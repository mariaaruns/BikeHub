
using BikeHub.Models;
using BikeHub.Service;
using BikeHub.Service.Interface;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BikeHub.Features
{
    public class UserModule : ICarterModule
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly RoleManager<ApplicationRole> _roleManager;


        public void AddRoutes(IEndpointRouteBuilder app)
        {
            //app.MapPost("/createUser", async ([FromForm] RegisterDto dto, [FromServices] UserManager<ApplicationUser> userManager) =>
            //{
            //    try
            //    {
            //        string newfilePath = string.Empty;

            //        if (dto.ImgFile?.Length > 0)
            //        {
            //            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), commonInfo.USER_IMG_PATH);
            //            if (!Directory.Exists(uploadsFolder))
            //                Directory.CreateDirectory(uploadsFolder);

            //            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.ImgFile.FileName)}";
            //            newfilePath = Path.Combine(uploadsFolder, fileName);

            //            using (var stream = new FileStream(newfilePath, FileMode.Create))
            //            {
            //                await dto.ImgFile.CopyToAsync(stream);
            //            }

            //            dto.Image = $"{fileName}";

            //        }

            //        var user = new ApplicationUser
            //        {
            //            FirstName = dto.FirstName,
            //            LastName = dto.LastName,
            //            UserName = dto.Email,
            //            NormalizedUserName = dto.Email.ToUpper(),
            //            Email = dto.Email,
            //            NormalizedEmail = dto.Email.ToUpper(),
            //            Image = dto.Image,
            //        };
            //        var result = await userManager.CreateAsync(user, dto.Password);

            //        if (result.Succeeded)
            //        {
            //            var id = await userManager.GetUserIdAsync(user);
            //            user.Id = int.Parse(id);
            //            await userManager.AddToRoleAsync(user, dto.Role);
            //            return Results.Ok(ApiResponse<string>.Success("User Register Successfully"));
            //        }
            //        else
            //        {

            //            var errors = result.Errors.Select(e => e.Description);

            //           return Results.BadRequest(ApiResponse<string>.Fail("User Register failed", errors: errors));
            //        }
            //    }
            //    catch(Exception ex) {

            //        return Results.InternalServerError(ApiResponse<string>.Fail("User Register failed",ex.Message));

            //    }

            //}).WithTags("Users").DisableAntiforgery();
            
            //app.MapPost("/login", async ([FromBody] LoginDto dto,[FromServices] UserManager<ApplicationUser> userManager,[FromServices] IConfiguration config, HttpContext http) =>
            //{
            //    if (dto is null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            //    {
            //        return Results.BadRequest(ApiResponse<JwtResponse>.Fail("Invalid request", errors: new[] { "Email and Password are required." }));
            //    }

            //    var user = await userManager.FindByEmailAsync(dto.Email);
            //    if (user is null)
            //    {
            //        return Results.BadRequest(ApiResponse<JwtResponse>.Fail("Invalid credentials", errors: new[] { "Invalid email or password." }));
            //    }

            //    var passwordValid = await userManager.CheckPasswordAsync(user, dto.Password);
            //    if (!passwordValid)
            //    {
            //        return Results.BadRequest(ApiResponse<JwtResponse>.Fail("Invalid credentials", errors: new[] { "Invalid email or password." }));
            //    }

            //    // Build claims
            //    var claims = new List<Claim>
            //    {
            //        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            //        new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            //        new Claim("name", $"{user.FirstName} {user.LastName}".Trim())
            //    };

            //    var roles = await userManager.GetRolesAsync(user);
            //    foreach (var role in roles)
            //    {
            //        claims.Add(new Claim(ClaimTypes.Role, role));
            //    }

            //    // Read JWT settings from configuration
            //    var keySecret = config["Jwt:Key"];
            //    var issuer = config["Jwt:Issuer"];
            //    var audience = config["Jwt:Audience"];
            //    var expiresMinutes = int.TryParse(config["Jwt:ExpiresMinutes"], out var m) ? m : 60;

            //    if (string.IsNullOrWhiteSpace(keySecret))
            //    {
            //        return Results.Problem("JWT configuration is missing (Jwt:Key).");
            //    }

            //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keySecret));
            //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //    var expires = DateTime.UtcNow.AddMinutes(expiresMinutes);

            //    var token = new JwtSecurityToken(
            //        issuer: issuer,
            //        audience: audience,
            //        claims: claims,
            //        expires: expires,
            //        signingCredentials: creds);

            //    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            //    var payload = new JwtResponse
            //    {
            //        Token = tokenString,
            //        Expires = expires,
            //        Email = user.Email,
            //        Name = $"{user.FirstName} {user.LastName}".Trim(),
            //        ProfileImage= $"/{commonInfo.USER_IMG_PATH}/{user.Image}"
            //    };

            //    return Results.Ok(ApiResponse<JwtResponse>.Success(payload));

            //}).WithTags("Users").DisableAntiforgery();

            //app.MapGet("/users", async ([AsParameters]  UsersRequestDto req, [FromServices] IApplicationUserStore<ApplicationUser> userStore) =>
            //{
            //    try
            //    {
            //        var IsValid = ModelValidator.Validate(req);

            //        if (!IsValid.IsValid)
            //        {
            //            return Results.BadRequest(ApiResponse<string>.Fail("Invalid Request", IsValid.Errors));
            //        }

            //        var pagedUsers = await userStore.GetAllUsersAsync(req, CancellationToken.None);

            //        if (pagedUsers.Data is { })
            //        {
            //            return Results.Ok(ApiResponse<PagedResult<UsersDto>>.Success(pagedUsers));
            //        }

            //        return Results.BadRequest(ApiResponse<PagedResult<UsersDto>>.Fail("No users list", IsValid.Errors));
            //    }
            //    catch (Exception ex)
            //    {
            //        return Results.InternalServerError(ApiResponse<PagedResult<UsersDto>>.Fail("Failed to retreive users list", ex.Message));
            //        throw;
            //    }



            //}).WithTags("Users").DisableAntiforgery();


        }
    }

}

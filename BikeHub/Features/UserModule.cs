using BikeHub.Models;
using BikeHub.Service;
using BikeHub.Shared.Dto.Request;
using Carter;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BikeHub.Features
{
    public class UserModule : ICarterModule
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

       
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/createUser", async ([FromForm] RegisterDto  dto, [FromServices] UserManager<ApplicationUser> userManager) =>
            {
                var user = new ApplicationUser
                {
                    
                    UserName = dto.Email,
                    NormalizedUserName = dto.Email.ToUpper(),
                    Email = dto.Email,
                    NormalizedEmail =dto.Email.ToUpper(),
                    Image= dto.Image,
                };


            }).WithTags("Users");


        }
    }
}

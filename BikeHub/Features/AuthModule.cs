using BikeHub.Extension;
using BikeHub.Service.Interface;
using BikeHub.Shared.Common;
using BikeHub.Shared.Dto.Request;
using BikeHub.Shared.Dto.Request.AuthDto;
using BikeHub.Shared.Dto.Response;
using BikeHub.Shared.Dto.Response.Auth;
using Carter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;


namespace BikeHub.Features
{
    public class AuthModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/user/register", async ([FromBody] CreateUserDto dto, [FromServices] IAuthService authService) =>
            {
                try
                {
                   var validate=ModelValidator.Validate(dto);
                    if (!validate.IsValid)
                    {
                        var error = string.Join($",{Environment.NewLine} ", validate.Errors);
                        return Results.UnprocessableEntity(ApiResponse<string>.Fail(error));
                    }


                    var isUserAlreadyExists = await authService.GetUserByUserName(dto.UserName);

                    if (isUserAlreadyExists != null)
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail("User already exists!"));
                    }
                
                    var extension = ImageHelper.GetImageExtension(dto.Imagebyte);

                    if (string.IsNullOrEmpty(extension))
                    {
                        throw new Exception("Invalid image format");
                    }

                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), commonInfo.USER_IMG_PATH);
                    
                    var fileName=await ImageHelper.SaveImageAsync(dto.Imagebyte,
                                                folderPath,
                                                dto.UserName,
                                                extension);


                    dto.Image = fileName;
                    var created = await authService.CreateUser(dto);
                    if(!created)
                        return Results.BadRequest(ApiResponse<string>.Fail("User creation failed"));

                    return Results.Ok(ApiResponse<string>.Success("User created"));
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail(ex.Message));
                }
            })
                .WithTags("Auth & User Permission")
                .DisableAntiforgery();

            app.MapPost("/api/user/login", async ([FromBody] LoginDto dto, [FromServices] IAuthService authService) =>
            {
                try
                {
                    var result = await authService.GenerateJwtTokenAndPolicyAsync(dto);

                    var identityErrorResult = result.Item1;

                    if (!identityErrorResult.IsSuccess)
                    {
                        //return Results.BadRequest(ApiResponse<JwtResponse>.Fail());

                        return Results.Json(ApiResponse<JwtResponse>.Fail(identityErrorResult.message),
                                          statusCode: StatusCodes.Status401Unauthorized);
                    }

                    if (result is { Item2.Token: null }) {

                        return Results.BadRequest(ApiResponse<JwtResponse>.Fail("Error occured while authenticating.."));
                    }
                    return Results.Ok(ApiResponse<JwtResponse>.Success(result.Item2));
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }

            })
                .WithTags("Auth & User Permission")
                .DisableAntiforgery();

            app.MapPost("/api/user/logout", (HttpContext context, IMemoryCache cache) =>
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId != null)
                {
                    cache.Remove($"POLICIES_{userId}");
                }

                return Results.Ok("Logged out successfully");
            })
                .WithTags("Auth & User Permission")
                .DisableAntiforgery();

            app.MapGet("/api/user/{userId:long}", async (long userId,IAuthService authService) =>
            {
                if (userId <= 0)
                {
                    return Results.BadRequest(ApiResponse<UserByIdDto>.Fail("Invalid user ID"));
                }
                try
                {
                    var user = await authService.GetUserById(userId);

                    if (user == null)
                    {
                        return Results.NotFound(ApiResponse<UsersDto>.Fail("User not found"));
                    }
                    user.Image = user.Image != null ? Path.Combine(commonInfo.BaseUrl, commonInfo.USER_IMG_PATH, user.Image) : null;

                    return Results.Ok(ApiResponse<UserByIdDto>.Success(user));

                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<UserByIdDto>.Fail("Internal server error, " + ex.Message));
                }

            })
                .WithTags("Auth & User Permission").DisableAntiforgery(); 

            app.MapGet("/api/userPolicy/{userId:long}", async (long userId, [FromServices] IAuthService authService) =>
            {
                try
                {
                    var result = await authService.GetAllPoliciesByUserId(userId);
                    
                    return Results.Ok(ApiResponse<List<UserPolicyResponse>>.Success(result));

                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<List<UserPolicyResponse>>.Fail("Internal server error!"));
                }
            })
                .WithTags("Auth & User Permission").DisableAntiforgery(); 

            app.MapPost("/api/userPolicy/{userId:long}/ApplyPolicy", async (long userId, [FromBody] ApplyPolicyDto[] dto, [FromServices] IAuthService authService) =>
            {
                try
                {
                    var result = await authService.AddOrEditPolicyForUser(userId, dto);

                    if (result)
                    {
                        return Results.Ok(ApiResponse<string>.Success("Policy applied successfully"));
                    }
                    else
                    {
                        return Results.BadRequest(ApiResponse<string>.Fail("Failed to apply policy"));
                    }
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("Internal server error,try again"));

                }
            })
                .WithTags("Auth & User Permission").DisableAntiforgery();

            app.MapGet("/api/roles", async ([FromServices] IAuthService _authService) =>
            {
                try
                {
                    var result = await _authService.GetAllRoles();

                    return Results.Ok(ApiResponse<RolesDto[]>.Success(result));
                }
                catch (Exception)
                {
                    return Results.InternalServerError(ApiResponse<RolesDto[]>.Fail("Failed to retrieve roles"));
                }

            })
                .WithTags("Auth & User Permission").DisableAntiforgery();

            app.MapGet("/api/userPolicyCached", async (HttpContext context, IAuthService authService) =>
                {
                    var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (!long.TryParse(userIdClaim, out long userId))
                        return Results.BadRequest(ApiResponse<HashSet<string>>.Fail("Invalid user"));

                    var policies = await authService.GetPoliciesAsync(userId);

                    return Results.Ok(ApiResponse<HashSet<string>>.Success(policies));

                }).WithTags("Auth & User Permission").DisableAntiforgery();

            app.MapPut("/api/user/update", async ([FromBody] UpdateUserDto dto, [FromServices] IAuthService authService) =>
            {
                try
                {
                    var validate = ModelValidator.Validate(dto);
                    if (!validate.IsValid)
                    {
                        var error = string.Join($",{Environment.NewLine} ", validate.Errors);
                        return Results.UnprocessableEntity(ApiResponse<string>.Fail(error));
                    }
                    var result = await authService.UpdateUser(dto);
                    if (result)
                        return Results.Ok(ApiResponse<string>.Success("User updated successfully"));
                    else
                        return Results.BadRequest(ApiResponse<string>.Fail("Failed to update user"));
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<string>.Fail("Internal server error, " + ex.Message));
                }
            }).WithTags("Auth & User Permission").DisableAntiforgery();


            app.MapPost("/api/users", async ([FromBody] UsersRequestDto dto, [FromServices] IAuthService authService) =>
            {
                try
                {


                    var result = await authService.GetAllUser(dto);

                    if (result is not null) {

                        result.Data.ForEach(x =>
                        {
                            if (x.Image != null)
                                x.Image = Path.Combine(commonInfo.BaseUrl,commonInfo.USER_IMG_PATH,x.Image);
                        });
                    }

                    return Results.Ok(ApiResponse<PagedResult<UsersDto>>.Success(result));
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(ApiResponse<PagedResult<UsersDto>>.Fail("Internal server error, " + ex.Message));
                }
            }).WithTags("Auth & User Permission").DisableAntiforgery();
        }
    }
}

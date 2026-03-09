using BikeHub.AuthHandlers;
using BikeHub.AuthHandlers.PolicyHandler;
using BikeHub.Models;
using BikeHub.Repository;
using BikeHub.Repository.IRepository;
using BikeHub.Service;
using BikeHub.Service.Interface;
using BikeHub.Shared.Dto.Request;
using Carter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Serilog;
using System.Data;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddMemoryCache();
builder.Services.AddOpenApi();
builder.Services.AddCarter();

builder.Services.AddTransient<IDbConnection>(sp =>new SqlConnection(builder.Configuration.GetConnectionString("Conn")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository,OrderRepository >();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IDashboardRepository,DashboardRepository>();
//builder.Services.AddTransient<IApplicationUserStore<ApplicationUser>, UserStore>();
//builder.Services.AddTransient<IRoleStore<ApplicationRole>, RoleStore>();
//builder.Services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthorizationHandler, PolicyHandler>();

//builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
//{
//    options.Password.RequireDigit = true;
//    options.Password.RequiredLength = 6;
//    options.Password.RequireNonAlphanumeric = false;
//    options.Password.RequireUppercase = true;
//    options.Password.RequireLowercase = true;
//    //options.User.RequireUniqueEmail = true;
//}).AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = ctx =>
    {
        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = ctx =>
    {
        ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

// Configure authentication to use JWT as the default scheme (authenticate & challenge)

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = builder.Configuration.GetValue<string>("jwt:key");
    var keyByteArray = Encoding.UTF8.GetBytes(key);
    var securityKey = new SymmetricSecurityKey(keyByteArray);
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = securityKey,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        RoleClaimType= ClaimTypes.Role

    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("ADMIN"));
    options.AddPolicy("AdminOrStaff", policy => policy.RequireRole("ADMIN", "STAFF"));
    options.AddPolicy("AdminOrStaffOrMechanic", policy => policy.RequireRole("ADMIN", "STAFF","MECHANIC"));

    options.AddPolicy("PRODUCT_ADD",
        p => p.Requirements.Add(new PolicyRequirement("PRODUCT_ADD")));

    options.AddPolicy("PRODUCT_DELETE",
        p => p.Requirements.Add(new PolicyRequirement("PRODUCT_DELETE")));

    options.AddPolicy("PRODUCT_EDIT",
        p => p.Requirements.Add(new PolicyRequirement("PRODUCT_EDIT")));

    options.AddPolicy("PRODUCT_VIEW",
        p => p.Requirements.Add(new PolicyRequirement("PRODUCT_VIEW")));


    options.AddPolicy("CUSTOMER_VIEW",
        p => p.Requirements.Add(new PolicyRequirement("CUSTOMER_VIEW")));

    options.AddPolicy("CUSTOMER_EDIT",
        p => p.Requirements.Add(new PolicyRequirement("CUSTOMER_EDIT")));

    options.AddPolicy("CUSTOMER_DELETE",
        p => p.Requirements.Add(new PolicyRequirement("CUSTOMER_DELETE")));

    options.AddPolicy("CUSTOMER_ADD",
        p => p.Requirements.Add(new PolicyRequirement("CUSTOMER_ADD")));


    options.AddPolicy("ORDER_ADD",
        p => p.Requirements.Add(new PolicyRequirement("ORDER_ADD")));

    options.AddPolicy("ORDER_UPDATE",
        p => p.Requirements.Add(new PolicyRequirement("ORDER_UPDATE")));

    options.AddPolicy("ORDER_VIEW",
        p => p.Requirements.Add(new PolicyRequirement("ORDER_VIEW")));


    options.AddPolicy("DASHBOARD_VIEW",
        p => p.Requirements.Add(new PolicyRequirement("DASHBOARD_VIEW")));



    options.AddPolicy("USER_VIEW",
        p => p.Requirements.Add(new PolicyRequirement("USER_VIEW")));

    options.AddPolicy("USER_DELETE",
        p => p.Requirements.Add(new PolicyRequirement("USER_DELTE")));
    
    options.AddPolicy("USER_EDIT",
        p => p.Requirements.Add(new PolicyRequirement("USER_EDIT")));
    options.AddPolicy("USER_ADD",
        p => p.Requirements.Add(new PolicyRequirement("USER_ADD")));


});


builder.Services.AddAntiforgery(options =>
{
    options.SuppressXFrameOptionsHeader = true; // optional
});




builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid JWT token."
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File(
        path: "Logs/app-log.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();
app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}
app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Content")),
    RequestPath = "/Content"
});

app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();
app.Run();




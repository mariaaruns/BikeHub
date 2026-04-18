using Bikehub.Hybrid.Authhandler;
using Bikehub.Hybrid.DeviceServices.Location;
using Bikehub.Hybrid.DeviceServices.Toast;
using Bikehub.Hybrid.Services.Http.Auth;
using Bikehub.Hybrid.Services.Http.ServiceDashboard;
using CommunityToolkit.Maui;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace Bikehub.Hybrid
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                .UseMauiCommunityToolkit(options => options.SetShouldEnableSnackbarOnWindows(true))
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            //builder.Services.AddScoped<NavigationManager>();
            builder.Services.AddScoped<CustomAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(
                sp => sp.GetRequiredService<CustomAuthStateProvider>());
            builder.Services.AddSingleton<UserSession>();
            
            builder.Services.AddTransient<AuthTokenHandler>();
            builder.Services.AddTransient<AuthorizedHandler>();
            builder.Services.AddSingleton<ILocationService, LocationService>();
            builder.Services.AddSingleton<IToastService, ToastService>();

            builder.Services.AddHttpClient("BikeHub")
            .ConfigureHttpClient(client =>
                client.BaseAddress = new Uri("https://hms704v0-7079.inc1.devtunnels.ms"))
            .AddHttpMessageHandler<AuthTokenHandler>()      // Adds token to requests
            .AddHttpMessageHandler<AuthorizedHandler>();   // Handles 401 responses





            //Api service registrations

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IServiceDashboard, ServiceDashboard>();





            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("SERVICE_DASHBOARD", p =>
                    p.RequireClaim("Permission", "SERVICE_DASHBOARD"));
            });
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

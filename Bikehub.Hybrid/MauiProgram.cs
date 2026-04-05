using Bikehub.Hybrid.Authhandler;
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
            builder
                .UseMauiApp<App>()
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

            builder.Services.AddHttpClient("BikeHub")
            .ConfigureHttpClient(client =>
                client.BaseAddress = new Uri("https://hms704v0-7079.inc1.devtunnels.ms"))
            .AddHttpMessageHandler<AuthTokenHandler>()      // Adds token to requests
            .AddHttpMessageHandler<AuthorizedHandler>();   // Handles 401 responses

            //builder.Services.AddScoped<HttpClient>(sp =>
            //{
            //    var authHandler = sp.GetRequiredService<AuthTokenHandler>();
            //    authHandler.InnerHandler = new HttpClientHandler(); 

            //    return new HttpClient(authHandler)
            //    {
            //        BaseAddress = new Uri("https://hms704v0-7079.inc1.devtunnels.ms")
            //    };
            //});

         //   builder.Services.AddAuthorizationCore();
            builder.Services.AddAuthorizationCore(options =>
            {
                // Register the client-side policy that maps to your server policy name
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

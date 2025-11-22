using BikeHub.Mobile.ApiServices;
using BikeHub.Mobile.Handler;
using BikeHub.Mobile.Pages;
using BikeHub.Mobile.ViewModel;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Handlers;
using Refit;
using SkiaSharp.Extended.UI;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Syncfusion.Maui.Toolkit.Hosting;
using System.Runtime.Intrinsics.Arm;
namespace BikeHub.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var apiBase =  "https://f405rch9-7079.inc1.devtunnels.ms";
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                
                .ConfigureSyncfusionToolkit()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddTransient<AuthHandler>();
            static void AddApiClient<TApi>(MauiAppBuilder b, string baseUrl) where TApi : class
            {
                b.Services
                 .AddRefitClient<TApi>()
                 .AddHttpMessageHandler<AuthHandler>()
                 .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl));
            }

            //builder.Services.AddRefitClient<IUserApi>()
            //           .AddHttpMessageHandler<AuthHandler>()
            //           .ConfigureHttpClient(c =>
            //           {
            //               c.BaseAddress = new Uri("https://f405rch9-7079.inc1.devtunnels.ms");
            //           });
            AddApiClient<IUserApi>(builder, apiBase);
            AddApiClient<IDashBoardApi>(builder, apiBase);


            builder.Services.AddTransient<LoginPage>().AddTransient<LoginViewModel>();
            builder.Services.AddTransient<MainPage>().AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<ProductsPage>().AddTransient<ProductsViewModel>();
            builder.Services.AddTransient<CustomersPage>().AddTransient<CustomerViewModel>();
            builder.Services.AddTransient<OrdersPage>().AddTransient<OrderViewModel>();
            builder.Services.AddTransient<UsersPage>().AddTransient<UserViewModel>();
            builder.Services.AddTransient<AddEditProductPage>();
            builder.Services.AddTransient<AddEditCategory>();
            builder.Services.AddTransient<AddEditBrand>();
            builder.Services.AddTransient<AddEditOrders>();
            builder.Services.AddTransient<AddEditUsers>();
            builder.Services.AddTransient<AddEditCustomer>();
            builder.Services.AddTransient<OrderPlacedPage>();
            builder.Services.AddTransient<OrderCheckoutPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }


}

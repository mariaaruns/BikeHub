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

            //Api Client Registrations with Refit and AuthHandler
            builder.Services.AddTransient<AuthHandler>();
            static void AddApiClient<TApi>(MauiAppBuilder b, string baseUrl) where TApi : class
            {
                b.Services
                 .AddRefitClient<TApi>()
                 .AddHttpMessageHandler<AuthHandler>()
                 .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl));
            }

            AddApiClient<IUserApi>(builder, apiBase);
            AddApiClient<IDashBoardApi>(builder, apiBase);
            AddApiClient<IProductApi>(builder, apiBase);
            //AddApiClient<ICategoryApi>(builder, apiBase);
            //AddApiClient<IBrandApi>(builder, apiBase);



            //Page and ViewModel Registrations
            builder.Services.AddTransient<LoginPage>().AddTransient<LoginViewModel>();
            builder.Services.AddTransient<MainPage>().AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<ProductsPage>().AddTransient<ProductsViewModel>();
            builder.Services.AddTransient<CustomersPage>().AddTransient<CustomerViewModel>();
            builder.Services.AddTransient<OrdersPage>().AddTransient<OrderViewModel>();
            builder.Services.AddTransient<UsersPage>().AddTransient<UserViewModel>();
            builder.Services.AddTransient<AddEditProductPage>().AddTransient<AddEditProductViewModel>();
            builder.Services.AddTransient<AddEditCategory>().AddTransient<AddEditCategoryViewModel>();
            builder.Services.AddTransient<AddEditBrand>().AddTransient<AddEditBrandViewModel>();
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

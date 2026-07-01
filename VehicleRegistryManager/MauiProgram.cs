using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using VehicleRegistryManager.Area.Authentication.View;
using VehicleRegistryManager.Area.Authentication.ViewModel;
using VehicleRegistryManager.Area.Dashboard.ViewModel;
using VehicleRegistryManager.Area.VehicleCategories.ViewModel;
using VehicleRegistryManager.Infrastructure.Data;
using VehicleRegistryManager.Infrastructure.Services;

namespace VehicleRegistryManager
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureMauiHandlers(handlers =>
                {
#if IOS
                    Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoBorder", (handler, _) =>
                        handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None);

                    Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("NoBorder", (handler, _) =>
                        handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None);

                    Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping("NoBorder", (handler, _) =>
                    {
                        handler.PlatformView.Layer.BorderWidth = 0;
                        handler.PlatformView.Layer.BorderColor = UIKit.UIColor.Clear.CGColor;
                    });
#endif
                });

            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddTransient<IDependencyObj, DependencyObj>();
            builder.Services.AddSingleton<VehicleRepository>();

            builder.Services.AddTransient<LoadingViewModel>();
            builder.Services.AddTransient<LoadingView>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<VehicleCategoryViewModel>();
            builder.Services.AddTransient<AddAndEditVehicleViewModel>();
            builder.Services.AddTransient<VehicleListViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

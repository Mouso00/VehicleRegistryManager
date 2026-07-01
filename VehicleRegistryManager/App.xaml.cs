using Microsoft.Extensions.DependencyInjection;
using VehicleRegistryManager.Area.Authentication.View;

namespace VehicleRegistryManager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var loadingView = IPlatformApplication.Current!.Services.GetRequiredService<LoadingView>();
            return new Window(loadingView);
        }
    }
}

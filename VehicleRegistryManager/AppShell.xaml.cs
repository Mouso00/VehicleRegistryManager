
using VehicleRegistryManager.Area.Dashboard.View;
using VehicleRegistryManager.Area.VehicleCategories.View;
using VehicleRegistryManager.Infrastructure;

namespace VehicleRegistryManager
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }
        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(DashboardView), typeof(DashboardView));
            Routing.RegisterRoute(nameof(VehicleCategoryView), typeof(VehicleCategoryView));
            Routing.RegisterRoute(nameof(AddAndEditVehicleView), typeof(AddAndEditVehicleView));
            Routing.RegisterRoute(nameof(VehicleListView), typeof(VehicleListView));

        }
    }
}

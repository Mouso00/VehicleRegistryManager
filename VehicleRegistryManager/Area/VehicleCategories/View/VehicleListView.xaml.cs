using VehicleRegistryManager.Area.VehicleCategories.ViewModel;
using VehicleRegistryManager.Infrastructure.BaseUI;

namespace VehicleRegistryManager.Area.VehicleCategories.View;

public partial class VehicleListView : BaseContentPage
{
    public VehicleListView(VehicleListViewModel vehicleListViewModel)
    {
        BindingContext = vehicleListViewModel;
        InitializeComponent();
    }
}

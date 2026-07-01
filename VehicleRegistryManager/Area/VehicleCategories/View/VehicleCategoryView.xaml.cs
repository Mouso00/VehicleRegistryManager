using VehicleRegistryManager.Area.VehicleCategories.ViewModel;
using VehicleRegistryManager.Infrastructure.BaseUI;

namespace VehicleRegistryManager.Area.VehicleCategories.View;

public partial class VehicleCategoryView : BaseContentPage
{
	public VehicleCategoryView(VehicleCategoryViewModel vehicleCategoryViewModel)
	{
		BindingContext  = vehicleCategoryViewModel;
        InitializeComponent();
	}
}
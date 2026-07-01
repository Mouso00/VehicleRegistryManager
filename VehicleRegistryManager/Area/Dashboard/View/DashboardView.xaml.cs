using VehicleRegistryManager.Area.Dashboard.ViewModel;
using VehicleRegistryManager.Infrastructure.BaseUI;

namespace VehicleRegistryManager.Area.Dashboard.View;

public partial class DashboardView : BaseContentPage
{
	public DashboardView(DashboardViewModel dashboardViewModel)
	{
		BindingContext = dashboardViewModel;
		InitializeComponent();
	}
}
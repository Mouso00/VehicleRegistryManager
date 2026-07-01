using VehicleRegistryManager.Area.Authentication.ViewModel;
using VehicleRegistryManager.Infrastructure.BaseUI;

namespace VehicleRegistryManager.Area.Authentication.View;

public partial class LoadingView : BaseContentPage
{
    public LoadingView(LoadingViewModel loadingViewModel)
    {
        BindingContext = loadingViewModel;
        InitializeComponent();
    }
}

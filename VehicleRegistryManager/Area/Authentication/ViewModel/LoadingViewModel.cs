using System.Threading.Tasks;
using VehicleRegistryManager.Infrastructure.BaseUI;

namespace VehicleRegistryManager.Area.Authentication.ViewModel;

public class LoadingViewModel : BasePageViewModel
{
    public LoadingViewModel(IDependencyObj dependencyObj) : base(dependencyObj)
    {
    }

    public override async Task OnAppearingAsync()
    {
        await Task.Delay(3000);

        if (Application.Current?.Windows.Count > 0)
            Application.Current.Windows[0].Page = new AppShell();
    }
}

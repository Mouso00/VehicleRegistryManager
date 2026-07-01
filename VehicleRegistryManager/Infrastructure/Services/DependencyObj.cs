namespace VehicleRegistryManager.Infrastructure.Services;

public class DependencyObj : IDependencyObj
{
    public INavigationService NavigationService { get; }

    public DependencyObj(INavigationService navigationService)
    {
        NavigationService = navigationService;
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace VehicleRegistryManager.Infrastructure.Services;

public class NavigationService : INavigationService
{
    public const string ParametersKey = "parameters";

    public Task NavigateBackAsync()
    {
        return Shell.Current.GoToAsync("..");
    }

    public Task NavigateToAsync<TView>() where TView : Page
    {
        return Shell.Current.GoToAsync(typeof(TView).Name);
    }

    public Task NavigateToAsync<TView>(IParameters parameters) where TView : Page
    {
        var query = new Dictionary<string, object>
        {
            { ParametersKey, parameters }
        };

        return Shell.Current.GoToAsync(typeof(TView).Name, query);
    }
}

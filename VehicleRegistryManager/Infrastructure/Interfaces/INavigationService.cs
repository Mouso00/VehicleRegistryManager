using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace VehicleRegistryManager.Infrastructure.Interfaces;

public interface INavigationService
{
    Task NavigateBackAsync();

    Task NavigateToAsync<TView>() where TView : Page;

    Task NavigateToAsync<TView>(IParameters parameters) where TView : Page;
}

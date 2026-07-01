using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRegistryManager.Infrastructure.Interfaces;

public interface IBasePageViewModel
{
    IParameters? Parameters { get; set; }   

    Task InitializeAsync();

    Task OnNavigatedToAsync();

    Task OnNavigatedFromAsync();

    Task OnApplyParametersAsync(IParameters? parameters);

    Task OnAppearingAsync();

    Task OnDisappearingAsync();
}

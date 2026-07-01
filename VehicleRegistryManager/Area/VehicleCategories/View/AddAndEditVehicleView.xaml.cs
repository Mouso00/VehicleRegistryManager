using CommunityToolkit.Maui.Extensions;
using VehicleRegistryManager.Area.VehicleCategories.ViewModel;
using VehicleRegistryManager.Controls;
using VehicleRegistryManager.Infrastructure.BaseUI;

namespace VehicleRegistryManager.Area.VehicleCategories.View;

public partial class AddAndEditVehicleView : BaseContentPage
{
    public AddAndEditVehicleView(AddAndEditVehicleViewModel addAndEditVehicleViewModel)
    {
        BindingContext = addAndEditVehicleViewModel;
        InitializeComponent();
    }

    private async void OnPickYearTapped(object sender, TappedEventArgs e)
    {
        if (BindingContext is not AddAndEditVehicleViewModel viewModel)
            return;

        var popup = new YearPickerPopup(viewModel.Year);
        var result = await this.ShowPopupAsync<string>(popup);

        // Fill the Year entry only when the user confirmed a value.
        if (!result.WasDismissedByTappingOutsideOfPopup && !string.IsNullOrEmpty(result.Result))
            viewModel.Year = result.Result;
    }
}

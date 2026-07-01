using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using VehicleRegistryManager.Area.VehicleCategories.View;
using VehicleRegistryManager.Infrastructure.BaseUI;
using VehicleRegistryManager.Infrastructure.Data;
using VehicleRegistryManager.Model;
using VehicleRegistryManager.Model.Parameters;

namespace VehicleRegistryManager.Area.VehicleCategories.ViewModel
{
    public partial class VehicleCategoryViewModel : BasePageViewModel
    {
        private readonly VehicleRepository _repository;

        public ObservableRangeCollection<CategoryItem> Categories { get; } = new();

        public VehicleCategoryViewModel(IDependencyObj dependencyObj, VehicleRepository repository) : base(dependencyObj)
        {
            _repository = repository;
        }

        public override Task OnAppearingAsync() => RunSafeAsync(async () =>
        {
            var counts = await _repository.GetCategoryCountsAsync();

            var items = Enum.GetValues<VehicleCategory>()
                .Select(category => CategoryItem.Create(category, counts.GetValueOrDefault(category)));

            Categories.ReplaceRange(items);
        }, "Could not load categories.");

        [RelayCommand]
        private Task GoToAddVehicle() =>
            NavigationService.NavigateToAsync<AddAndEditVehicleView>(new AddAndEditVehicleParameter(false, new Vehicle()));

        [RelayCommand]
        private Task SelectCategory(VehicleCategory category) =>
            NavigationService.NavigateToAsync<VehicleListView>(new VehicleListParameter(category));
    }
}

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

namespace VehicleRegistryManager.Area.Dashboard.ViewModel
{
    public partial class DashboardViewModel : BasePageViewModel
    {
        private readonly VehicleRepository _repository;

        private int _totalVehicles;
        public int TotalVehicles
        {
            get => _totalVehicles;
            set => SetProperty(ref _totalVehicles, value);
        }

        private int _categoriesCount;
        public int CategoriesCount
        {
            get => _categoriesCount;
            set => SetProperty(ref _categoriesCount, value);
        }

        public ObservableRangeCollection<CategoryItem> Categories { get; } = new();

        public DashboardViewModel(IDependencyObj dependencyObj, VehicleRepository repository) : base(dependencyObj)
        {
            _repository = repository;
        }

        public override Task OnAppearingAsync() => RunSafeAsync(async () =>
        {
            var counts = await _repository.GetCategoryCountsAsync();

            var items = Enum.GetValues<VehicleCategory>()
                .Select(category => CategoryItem.Create(category, counts.GetValueOrDefault(category)));

            Categories.ReplaceRange(items);

            TotalVehicles = counts.Values.Sum();
            CategoriesCount = counts.Count(pair => pair.Value > 0);
        }, "Could not load the dashboard.");

        [RelayCommand]
        private Task GoToAddVehicle() =>
            NavigationService.NavigateToAsync<AddAndEditVehicleView>(new AddAndEditVehicleParameter(false, new Vehicle()));

        [RelayCommand]
        private Task SelectCategory(VehicleCategory category) =>
            NavigationService.NavigateToAsync<VehicleListView>(new VehicleListParameter(category));
    }
}

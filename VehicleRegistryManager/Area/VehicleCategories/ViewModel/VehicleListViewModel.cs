using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    public partial class VehicleListViewModel : BasePageViewModel
    {
        private readonly VehicleRepository _repository;

        // The full list loaded from the database (search filters from this).
        private List<Vehicle> _allVehicles = new();
        private CancellationTokenSource? _searchCts;

        private VehicleCategory _category = VehicleCategory.Cars;

        public string Title => _category.ToString();

        public string SearchPlaceholder => $"Search {_category.ToString().ToLowerInvariant()}...";

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    DebounceSearch();
            }
        }

        public ObservableRangeCollection<Vehicle> Vehicles { get; } = new();

        public bool IsEmpty => Vehicles.Count == 0;

        public VehicleListViewModel(IDependencyObj dependencyObj, VehicleRepository repository) : base(dependencyObj)
        {
            _repository = repository;
            Vehicles.CollectionChanged += (_, _) => OnPropertyChanged(nameof(IsEmpty));
        }

        public override Task OnApplyParametersAsync(IParameters? parameters)
        {
            if (parameters is VehicleListParameter vehicleListParameter)
            {
                _category = vehicleListParameter.Category;
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(SearchPlaceholder));
            }

            return Task.CompletedTask;
        }

        public override Task OnAppearingAsync() => RunSafeAsync(async () =>
        {
            _allVehicles = await _repository.GetByCategoryAsync(_category);
            ApplyFilter();
        }, "Could not load vehicles.");

        // Waits 1 second after the last keystroke, then filters.
        private async void DebounceSearch()
        {
            _searchCts?.Cancel();
            _searchCts = new CancellationTokenSource();
            var token = _searchCts.Token;

            try
            {
                await Task.Delay(1000, token);
            }
            catch (TaskCanceledException)
            {
                return;
            }

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var query = SearchText?.Trim();

            var filtered = string.IsNullOrEmpty(query)
                ? _allVehicles
                : _allVehicles
                    .Where(v => v.Brand.Contains(query, System.StringComparison.OrdinalIgnoreCase)
                             || v.Model.Contains(query, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();

            Vehicles.ReplaceRange(filtered);
        }

        [RelayCommand]
        private Task GoToAddVehicle() =>
            NavigationService.NavigateToAsync<AddAndEditVehicleView>(
                new AddAndEditVehicleParameter(false, new Vehicle { Category = _category }));

        [RelayCommand]
        private Task EditVehicle(Vehicle vehicle) =>
            NavigationService.NavigateToAsync<AddAndEditVehicleView>(
                new AddAndEditVehicleParameter(true, vehicle));

        [RelayCommand]
        private Task DeleteVehicle(Vehicle vehicle) => RunSafeAsync(async () =>
        {
            if (vehicle == null)
                return;

            if (!await ConfirmAsync("Delete vehicle", "Are you sure you want to delete this vehicle?"))
                return;

            await _repository.DeleteAsync(vehicle);
            _allVehicles.Remove(vehicle);
            Vehicles.Remove(vehicle);
        }, "Could not delete the vehicle.");
    }
}

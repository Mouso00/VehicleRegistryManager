using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using VehicleRegistryManager.Infrastructure.BaseUI;
using VehicleRegistryManager.Infrastructure.Data;
using VehicleRegistryManager.Model;
using VehicleRegistryManager.Model.Parameters;

namespace VehicleRegistryManager.Area.VehicleCategories.ViewModel
{
    public partial class AddAndEditVehicleViewModel : BasePageViewModel
    {
        private readonly VehicleRepository _repository;

        // The vehicle being added or edited.
        private Vehicle _vehicle = new();

        private string _title = "Add Vehicle";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _isEdit;
        public bool IsEdit
        {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

        private string _selectedCategory = string.Empty;
        public string SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value, onChanged: () =>
            {
                if (!string.IsNullOrWhiteSpace(_selectedCategory)) CategoryError = string.Empty;
            });
        }

        private string _brand = string.Empty;
        public string Brand
        {
            get => _brand;
            set => SetProperty(ref _brand, value, onChanged: () =>
            {
                if (!string.IsNullOrWhiteSpace(_brand)) BrandError = string.Empty;
            });
        }

        private string _model = string.Empty;
        public string Model
        {
            get => _model;
            set => SetProperty(ref _model, value, onChanged: () =>
            {
                if (!string.IsNullOrWhiteSpace(_model)) ModelError = string.Empty;
            });
        }

        private string _plateNumber = string.Empty;
        public string PlateNumber
        {
            get => _plateNumber;
            set => SetProperty(ref _plateNumber, value, onChanged: () =>
            {
                if (!string.IsNullOrWhiteSpace(_plateNumber)) PlateNumberError = string.Empty;
            });
        }

        private string _year = string.Empty;
        public string Year
        {
            get => _year;
            set => SetProperty(ref _year, value, onChanged: () => YearError = string.Empty);
        }

        private string _comments = string.Empty;
        public string Comments
        {
            get => _comments;
            set => SetProperty(ref _comments, value);
        }

        private string? _photoPath;
        public string? PhotoPath
        {
            get => _photoPath;
            set => SetProperty(ref _photoPath, value, onChanged: () =>
            {
                OnPropertyChanged(nameof(HasPhoto));
                OnPropertyChanged(nameof(HasNoPhoto));
            });
        }

        public bool HasPhoto => !string.IsNullOrEmpty(PhotoPath);

        public bool HasNoPhoto => !HasPhoto;

        private string _createdText = string.Empty;
        public string CreatedText
        {
            get => _createdText;
            set => SetProperty(ref _createdText, value);
        }

        private string _updatedText = string.Empty;
        public string UpdatedText
        {
            get => _updatedText;
            set => SetProperty(ref _updatedText, value);
        }

        private bool _hasUpdated;
        public bool HasUpdated
        {
            get => _hasUpdated;
            set => SetProperty(ref _hasUpdated, value);
        }

        // --- Validation error messages (empty = no error) ---

        private string _categoryError = string.Empty;
        public string CategoryError
        {
            get => _categoryError;
            set => SetProperty(ref _categoryError, value, onChanged: () => OnPropertyChanged(nameof(HasCategoryError)));
        }
        public bool HasCategoryError => !string.IsNullOrEmpty(CategoryError);

        private string _brandError = string.Empty;
        public string BrandError
        {
            get => _brandError;
            set => SetProperty(ref _brandError, value, onChanged: () => OnPropertyChanged(nameof(HasBrandError)));
        }
        public bool HasBrandError => !string.IsNullOrEmpty(BrandError);

        private string _modelError = string.Empty;
        public string ModelError
        {
            get => _modelError;
            set => SetProperty(ref _modelError, value, onChanged: () => OnPropertyChanged(nameof(HasModelError)));
        }
        public bool HasModelError => !string.IsNullOrEmpty(ModelError);

        private string _plateNumberError = string.Empty;
        public string PlateNumberError
        {
            get => _plateNumberError;
            set => SetProperty(ref _plateNumberError, value, onChanged: () => OnPropertyChanged(nameof(HasPlateNumberError)));
        }
        public bool HasPlateNumberError => !string.IsNullOrEmpty(PlateNumberError);

        private string _yearError = string.Empty;
        public string YearError
        {
            get => _yearError;
            set => SetProperty(ref _yearError, value, onChanged: () => OnPropertyChanged(nameof(HasYearError)));
        }
        public bool HasYearError => !string.IsNullOrEmpty(YearError);

        public List<string> Categories { get; } = new()
        {
            "Cars",
            "Trucks",
            "Tractors",
        };

        public AddAndEditVehicleViewModel(IDependencyObj dependencyObj, VehicleRepository repository) : base(dependencyObj)
        {
            _repository = repository;
        }

        public override Task OnApplyParametersAsync(IParameters? parameters)
        {
            if (parameters is AddAndEditVehicleParameter parameter)
            {
                _vehicle = parameter.Vehicle;
                IsEdit = parameter.IsEdit;
                Title = parameter.IsEdit ? "Edit Vehicle" : "Add Vehicle";

                // Load the vehicle's data into the form fields.
                // On add, leave the category empty so the user must choose one.
                SelectedCategory = parameter.IsEdit ? _vehicle.Category.ToString() : string.Empty;
                Brand = _vehicle.Brand;
                Model = _vehicle.Model;
                PlateNumber = _vehicle.PlateNumber;
                Year = _vehicle.Year?.ToString() ?? string.Empty;

                Comments = _vehicle.Comments ?? string.Empty;
                PhotoPath = _vehicle.PhotoPath;

                CreatedText = $"Created: {_vehicle.CreatedAtUtc:dd'/'MM'/'yyyy}";
                HasUpdated = _vehicle.UpdatedAtUtc.HasValue;
                UpdatedText = _vehicle.UpdatedAtUtc.HasValue
                    ? $"Last updated: {_vehicle.UpdatedAtUtc.Value:dd'/'MM'/'yyyy}"
                    : string.Empty;
            }

            return Task.CompletedTask;
        }

        [RelayCommand]
        private Task PickPhoto() => RunSafeAsync(async () =>
        {
            var photo = await MediaPicker.Default.PickPhotoAsync();
            if (photo == null)
                return;

            // Copy the picked file into the app's data folder so the path stays valid.
            var localPath = Path.Combine(FileSystem.AppDataDirectory, $"vehicle_{Guid.NewGuid():N}{Path.GetExtension(photo.FileName)}");

            using (var source = await photo.OpenReadAsync())
            using (var destination = File.Create(localPath))
                await source.CopyToAsync(destination);

            PhotoPath = localPath;
        }, "Could not load the photo.");

        // Sets a red error under each empty mandatory field. Returns true if all are filled.
        private async Task<bool> Validate()
        {
            CategoryError = string.IsNullOrWhiteSpace(SelectedCategory) ? "Category is required" : string.Empty;
            BrandError = string.IsNullOrWhiteSpace(Brand) ? "Brand is required" : string.Empty;
            ModelError = string.IsNullOrWhiteSpace(Model) ? "Model is required" : string.Empty;
            PlateNumberError = string.IsNullOrWhiteSpace(PlateNumber) ? "Plate number is required" : string.Empty;

            // Year is optional, but if provided it must be numeric and within 1900..current year.
            YearError = string.Empty;
            if (!string.IsNullOrWhiteSpace(Year)
                && (!int.TryParse(Year, out var enteredYear) || enteredYear < 1900 || enteredYear > DateTime.Now.Year))
            {
                YearError = $"Enter a year between 1900 and {DateTime.Now.Year}";
            }

            if (HasCategoryError || HasBrandError || HasModelError || HasPlateNumberError || HasYearError)
                return false;

            // Reject a plate number that already belongs to another vehicle.
            var plate = PlateNumber.Trim();
            var all = await _repository.GetAllAsync();
            var duplicate = all.Any(v => v.Id != _vehicle.Id
                && string.Equals(v.PlateNumber, plate, StringComparison.OrdinalIgnoreCase));

            if (duplicate)
            {
                PlateNumberError = "Plate number already exists";
                return false;
            }

            return true;
        }

        [RelayCommand]
        private Task Save() => RunSafeAsync(async () =>
        {
            if (!await Validate())
                return;

            _vehicle.Category = Enum.Parse<VehicleCategory>(SelectedCategory);
            _vehicle.Brand = Brand?.Trim() ?? string.Empty;
            _vehicle.Model = Model?.Trim() ?? string.Empty;
            _vehicle.PlateNumber = PlateNumber.Trim();
            _vehicle.Year = int.TryParse(Year, out var year) ? year : null;
            _vehicle.Comments = string.IsNullOrWhiteSpace(Comments) ? null : Comments.Trim();
            _vehicle.PhotoPath = PhotoPath;

            await _repository.SaveAsync(_vehicle);
            await NavigateBackAsync();
        }, "Could not save the vehicle.");

        [RelayCommand]
        private Task Delete() => RunSafeAsync(async () =>
        {
            if (!await ConfirmAsync("Delete vehicle", "Are you sure you want to delete this vehicle?"))
                return;

            if (_vehicle.Id != 0)
                await _repository.DeleteAsync(_vehicle);

            await NavigateBackAsync();
        }, "Could not delete the vehicle.");
    }
}

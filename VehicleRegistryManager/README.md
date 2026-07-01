# Vehicle Registry Manager

A cross-platform vehicle registry app built with **.NET MAUI**. It lets you browse vehicles grouped by category and add, edit, and delete them, with all data stored locally on the device using SQLite.

## Features

- **Dashboard** with category cards (Cars, Trucks, Tractors) showing the number of vehicles in each, plus quick stats (total vehicles, categories in use).
- **Vehicle list per category** with search (Brand/Model, debounced), inline Edit/Delete, and a photo/icon thumbnail per vehicle.
- **Add / Edit vehicle** form with fields: Category, Brand, Model, Plate Number (mandatory), Year (optional), Comments (optional), and a Photo.
- **Photo picker** – pick an image from the device gallery; the file is copied into app storage and its path saved with the vehicle.
- **Year picker** – type a year manually or pick one (1900–current) from a popup.
- **Validation** – inline red messages for missing mandatory fields (cleared as you type), duplicate plate-number detection, and year range checks.
- **Local persistence** – SQLite database with automatic `CreatedAtUtc` / `UpdatedAtUtc` timestamps and first-launch seed data (including bundled sample photos).
- **Loading screen** shown on startup before navigating to the dashboard.

## Project Structure

```
VehicleRegistryManager/
├── Area/                 # Feature areas (each: View + ViewModel)
│   ├── Authentication/   # LoadingView (startup splash)
│   ├── Dashboard/        # DashboardView (category cards + stats)
│   └── VehicleCategories/# VehicleCategoryView, VehicleListView, AddAndEditVehicleView
├── Infrastructure/       # App-wide plumbing (no feature logic)
│   ├── BaseUI/           # BaseContentPage, BasePageViewModel, INotifyPropertyChanged base
│   ├── Interfaces/       # INavigationService, IDependencyObj, IParameters, ...
│   ├── Services/         # NavigationService, DependencyObj
│   ├── Data/             # VehicleRepository (SQLite access + seeding)
│   └── Routes.cs         # Shell route names
├── Model/                # Vehicle, VehicleCategory, CategoryItem, navigation Parameters
├── Controls/             # Reusable UI (YearPickerPopup)
└── Resources/            # Images (SVG icons), Fonts, Raw (seed photos), Splash, AppIcon
```

- **`Area`** – one folder per screen/feature, each pairing a XAML `View` with its `ViewModel` (MVVM).
- **`Infrastructure`** – shared building blocks: base classes, navigation, dependency aggregation, and the data layer, kept separate from feature code.

## How to Run

**Requirements**

- Visual Studio 2026 with the **.NET MAUI** workload
- **.NET 10 SDK**
- An **Android** or **iOS** emulator/simulator (or a physical device)

**Steps**

1. Open `VehicleRegistryManager.sln` in Visual Studio 2026.
2. Restore NuGet packages (automatic on first build).
3. Select an Android or iOS target from the debug dropdown.
4. Press **F5** to build and run. On first launch the database is created and seeded with sample vehicles.

## NuGet Packages

| Package | Why it's used |
|---|---|
| **sqlite-net-pcl** | Simple async ORM for the local SQLite database (the `Vehicle` table and CRUD). |
| **SQLitePCLRaw.bundle_green** | Native SQLite provider required by sqlite-net-pcl on the target platforms. |
| **CommunityToolkit.Mvvm** | MVVM helpers – source-generated commands (`[RelayCommand]`) used throughout the ViewModels. |
| **CommunityToolkit.Maui** | UI toolkit – provides the `Popup` used by the year picker. |
| **Refractored.MvvmHelpers** | `ObservableRangeCollection` for efficient bulk list updates in the CollectionViews. |
| **SimpleToolkit.Core / SimpleToolkit.SimpleShell** | Shell/navigation helpers and safe-area handling for the base page. |
| **Microsoft.Extensions.Logging.Debug** | Debug logging during development. |

> Photo selection uses **`MediaPicker`** from .NET MAUI Essentials (built in – no extra package required).

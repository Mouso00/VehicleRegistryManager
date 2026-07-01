using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using SQLite;
using VehicleRegistryManager.Model;

namespace VehicleRegistryManager.Infrastructure.Data
{
    public class VehicleRepository
    {
        private SQLiteAsyncConnection? _connection;

        // Opens the database (once), creates the table and seeds sample data on first launch.
        private async Task<SQLiteAsyncConnection> GetConnectionAsync()
        {
            if (_connection != null)
                return _connection;

            var path = Path.Combine(FileSystem.AppDataDirectory, "vehicles.db3");
            _connection = new SQLiteAsyncConnection(path);

            await _connection.CreateTableAsync<Vehicle>();

            if (await _connection.Table<Vehicle>().CountAsync() == 0)
                await SeedAsync(_connection);

            return _connection;
        }

        public async Task<List<Vehicle>> GetAllAsync()
        {
            var db = await GetConnectionAsync();
            return await db.Table<Vehicle>().ToListAsync();
        }

        public async Task<List<Vehicle>> GetByCategoryAsync(VehicleCategory category)
        {
            var db = await GetConnectionAsync();
            return await db.Table<Vehicle>().Where(v => v.Category == category).ToListAsync();
        }

        public async Task SaveAsync(Vehicle vehicle)
        {
            var db = await GetConnectionAsync();

            if (vehicle.Id == 0)
            {
                vehicle.CreatedAtUtc = DateTime.UtcNow;
                await db.InsertAsync(vehicle);
            }
            else
            {
                vehicle.UpdatedAtUtc = DateTime.UtcNow;
                await db.UpdateAsync(vehicle);
            }
        }

        public async Task DeleteAsync(Vehicle vehicle)
        {
            var db = await GetConnectionAsync();
            await db.DeleteAsync(vehicle);
        }

        public async Task<Dictionary<VehicleCategory, int>> GetCategoryCountsAsync()
        {
            var all = await GetAllAsync();
            return all
                .GroupBy(v => v.Category)
                .ToDictionary(group => group.Key, group => group.Count());
        }

        private static async Task SeedAsync(SQLiteAsyncConnection db)
        {
            var seeds = CreateSeedData();

            // Stamp the created date and copy each bundled photo into app storage.
            foreach (var seed in seeds)
            {
                seed.Vehicle.CreatedAtUtc = DateTime.UtcNow;
                seed.Vehicle.PhotoPath = await CopySeedPhotoAsync(seed.PhotoAsset);
            }

            await db.InsertAllAsync(seeds.Select(seed => seed.Vehicle));
        }

        // Copies a photo bundled in Resources/Raw into app storage. Returns null if it's missing.
        private static async Task<string?> CopySeedPhotoAsync(string? assetName)
        {
            if (string.IsNullOrEmpty(assetName))
                return null;

            try
            {
                if (!await FileSystem.AppPackageFileExistsAsync(assetName))
                    return null;

                var destination = Path.Combine(FileSystem.AppDataDirectory, assetName);

                using var source = await FileSystem.OpenAppPackageFileAsync(assetName);
                using var target = File.Create(destination);
                await source.CopyToAsync(target);

                return destination;
            }
            catch
            {
                // If the bundled image can't be read/copied, just seed the vehicle without a photo.
                return null;
            }
        }

        // Each seed pairs a vehicle with an optional bundled photo file name (placed in Resources/Raw).
        private static List<(Vehicle Vehicle, string? PhotoAsset)> CreateSeedData()
        {
            return new List<(Vehicle, string?)>
            {
                // Cars
                (new Vehicle { Category = VehicleCategory.Cars, Brand = "Toyota", Model = "Corolla", Year = 2022, PlateNumber = "ABC-1024" }, "seed_toyota.png"),
                (new Vehicle { Category = VehicleCategory.Cars, Brand = "Tesla", Model = "Model 3", Year = 2024, PlateNumber = "EV-7781" }, "seed_tesla.png"),
                (new Vehicle { Category = VehicleCategory.Cars, Brand = "Honda", Model = "Civic", Year = 2021, PlateNumber = "HCV-3092" }, "seed_honda.png"),

                // Trucks
                (new Vehicle { Category = VehicleCategory.Trucks, Brand = "Ford", Model = "F-150", Year = 2020, PlateNumber = "FRD-1500" }, "seed_ford.jpg"),
                (new Vehicle { Category = VehicleCategory.Trucks, Brand = "Volvo", Model = "FH16", Year = 2019, PlateNumber = "VLV-1600" }, "seed_volvo.jpg"),

                // Tractors
                (new Vehicle { Category = VehicleCategory.Tractors, Brand = "John Deere", Model = "5075E", Year = 2021, PlateNumber = "JD-5075" }, null),
                (new Vehicle { Category = VehicleCategory.Tractors, Brand = "Massey Ferguson", Model = "240", Year = 2018, PlateNumber = "MF-2400" }, null),
            };
        }
    }
}

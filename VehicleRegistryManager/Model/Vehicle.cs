using System;
using System.IO;
using SQLite;

namespace VehicleRegistryManager.Model
{
    public class Vehicle
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public VehicleCategory Category { get; set; }

        public string Brand { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public string PlateNumber { get; set; } = string.Empty;

        public int? Year { get; set; }

        public string? Comments { get; set; }

        public string? PhotoPath { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public DateTime? UpdatedAtUtc { get; set; }

        // --- Display helpers (not stored in the database) ---

        [Ignore]
        public string Icon => Category switch
        {
            VehicleCategory.Cars => "car.png",
            VehicleCategory.Trucks => "truck.png",
            VehicleCategory.Tractors => "tractor.png",
            _ => "car.png",
        };

        [Ignore]
        public string YearText => Year.HasValue ? $"Year {Year}" : string.Empty;

        [Ignore]
        public string AddedText => $"Added: {CreatedAtUtc:dd'/'MM'/'yyyy}";

        [Ignore]
        public bool HasPhoto => !string.IsNullOrEmpty(PhotoPath) && File.Exists(PhotoPath);

        [Ignore]
        public bool HasNoPhoto => !HasPhoto;
    }
}

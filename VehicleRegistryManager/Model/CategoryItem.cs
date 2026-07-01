using Microsoft.Maui.Graphics;

namespace VehicleRegistryManager.Model
{
    public class CategoryItem
    {
        public VehicleCategory Category { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Count { get; set; }

        public string Icon { get; set; } = string.Empty;

        public Color IconBackground { get; set; } = Colors.Transparent;

        public string CountText => Count == 1 ? "1 vehicle" : $"{Count} vehicles";

        // Builds a display item for a category with its count from the database.
        public static CategoryItem Create(VehicleCategory category, int count) => category switch
        {
            VehicleCategory.Cars => new CategoryItem
            {
                Category = category, Name = "Cars", Count = count, Icon = "car.png", IconBackground = Color.FromArgb("#DCF3E4"),
            },
            VehicleCategory.Trucks => new CategoryItem
            {
                Category = category, Name = "Trucks", Count = count, Icon = "truck.png", IconBackground = Color.FromArgb("#D9E8FB"),
            },
            VehicleCategory.Tractors => new CategoryItem
            {
                Category = category, Name = "Tractors", Count = count, Icon = "tractor.png", IconBackground = Color.FromArgb("#FBF1CF"),
            },
            _ => new CategoryItem { Category = category, Name = category.ToString(), Count = count },
        };
    }
}

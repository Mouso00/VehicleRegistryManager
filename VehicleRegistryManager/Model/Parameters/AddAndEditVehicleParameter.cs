using VehicleRegistryManager.Infrastructure.Interfaces;

namespace VehicleRegistryManager.Model.Parameters;

public record AddAndEditVehicleParameter(bool IsEdit, Vehicle Vehicle) : IParameters;

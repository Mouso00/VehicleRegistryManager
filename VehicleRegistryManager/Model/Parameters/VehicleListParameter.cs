using VehicleRegistryManager.Infrastructure.Interfaces;

namespace VehicleRegistryManager.Model.Parameters;

public record VehicleListParameter(VehicleCategory Category) : IParameters;

using TransportSystem.Models;

namespace TransportSystem.Interfaces;

public interface IDriverRepository : IRepository<Driver>
{
    Task<Driver?> GetByIdentificationAsync(string identification);
    Task<IEnumerable<Driver>> GetAvailableAsync();
}

public interface IVehicleRepository : IRepository<Vehicle>
{
    Task<Vehicle?> GetByPlateAsync(string plate);
    Task<IEnumerable<Vehicle>> GetAvailableAsync();
}

public interface ITransportServiceRepository : IRepository<TransportService>
{
    Task<TransportService?> GetWithDetailsAsync(int id);
    Task<IEnumerable<TransportService>> GetAllWithDetailsAsync();
    Task<IEnumerable<TransportService>> GetByStatusAsync(ServiceStatus status);
}
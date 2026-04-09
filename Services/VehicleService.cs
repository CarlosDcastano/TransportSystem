using TransportSystem.Interfaces;
using TransportSystem.Models;

namespace TransportSystem.Services;

public class VehicleService
{
    private readonly IVehicleRepository _repo;

    public VehicleService(IVehicleRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result> RegisterVehicleAsync(string plate, VehicleType type, int capacity)
    {
        if (string.IsNullOrWhiteSpace(plate))
            return Result.Fail("La placa es obligatoria.");

        if (capacity <= 0)
            return Result.Fail("La capacidad debe ser mayor a cero.");

        var existing = await _repo.GetByPlateAsync(plate);
        if (existing is not null)
            return Result.Fail($"Ya existe un vehículo con placa {plate}.");

        var vehicle = new Vehicle
        {
            Plate    = plate.Trim().ToUpper(),
            Type     = type,
            Capacity = capacity,
            Status   = VehicleStatus.Available
        };

        await _repo.AddAsync(vehicle);
        return Result.Ok($"Vehículo {plate} registrado correctamente.");
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync() =>
        await _repo.GetAllAsync();

    public async Task<IEnumerable<Vehicle>> GetAvailableAsync() =>
        await _repo.GetAvailableAsync();
}
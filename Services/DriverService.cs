using TransportSystem.Interfaces;
using TransportSystem.Models;

namespace TransportSystem.Services;

public class DriverService
{
    private readonly IDriverRepository _repo;

    public DriverService(IDriverRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result> RegisterDriverAsync(string identification, string fullName, string license)
    {
        if (string.IsNullOrWhiteSpace(identification) ||
            string.IsNullOrWhiteSpace(fullName) ||
            string.IsNullOrWhiteSpace(license))
            return Result.Fail("Todos los campos son obligatorios.");

        var existing = await _repo.GetByIdentificationAsync(identification);
        if (existing is not null)
            return Result.Fail($"Ya existe un conductor con identificación {identification}.");

        var driver = new Driver
        {
            Identification = identification.Trim(),
            FullName       = fullName.Trim(),
            License        = license.Trim(),
            Status         = DriverStatus.Available
        };

        await _repo.AddAsync(driver);
        return Result.Ok($"Conductor '{fullName}' registrado correctamente.");
    }

    public async Task<IEnumerable<Driver>> GetAllAsync() =>
        await _repo.GetAllAsync();

    public async Task<IEnumerable<Driver>> GetAvailableAsync() =>
        await _repo.GetAvailableAsync();
}
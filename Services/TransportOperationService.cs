using TransportSystem.Interfaces;
using TransportSystem.Models;

namespace TransportSystem.Services;

public class TransportOperationService
{
    private readonly ITransportServiceRepository _serviceRepo;
    private readonly IDriverRepository           _driverRepo;
    private readonly IVehicleRepository          _vehicleRepo;

    private const decimal SurchargePerKmLong    = 500m;
    private const double  LongDistanceThreshold = 100.0;

    public TransportOperationService(
        ITransportServiceRepository serviceRepo,
        IDriverRepository driverRepo,
        IVehicleRepository vehicleRepo)
    {
        _serviceRepo = serviceRepo;
        _driverRepo  = driverRepo;
        _vehicleRepo = vehicleRepo;
    }

    public async Task<Result> RegisterServiceAsync(string origin, string destination, double distanceKm)
    {
        if (string.IsNullOrWhiteSpace(origin) || string.IsNullOrWhiteSpace(destination))
            return Result.Fail("Origen y destino son obligatorios.");

        if (distanceKm <= 0)
            return Result.Fail("La distancia debe ser mayor a cero.");

        var service = new TransportService
        {
            Origin      = origin.Trim(),
            Destination = destination.Trim(),
            DistanceKm  = distanceKm,
            Status      = ServiceStatus.Pending,
            TotalCost   = 0
        };

        await _serviceRepo.AddAsync(service);
        return Result.Ok($"Servicio registrado: {origin} → {destination}.");
    }

    public async Task<Result> AssignResourcesAsync(int serviceId, int driverId, int vehicleId)
    {
        var service = await _serviceRepo.GetWithDetailsAsync(serviceId);
        if (service is null)
            return Result.Fail($"No existe un servicio con ID {serviceId}.");

        if (service.Status != ServiceStatus.Pending)
            return Result.Fail("Solo se pueden asignar recursos a servicios pendientes.");

        if (service.DriverId.HasValue || service.VehicleId.HasValue)
            return Result.Fail("El servicio ya tiene recursos asignados.");

        var driver = await _driverRepo.GetByIdAsync(driverId);
        if (driver is null)
            return Result.Fail($"No existe conductor con ID {driverId}.");
        if (driver.Status != DriverStatus.Available)
            return Result.Fail($"El conductor {driver.FullName} no está disponible.");

        var vehicle = await _vehicleRepo.GetByIdAsync(vehicleId);
        if (vehicle is null)
            return Result.Fail($"No existe vehículo con ID {vehicleId}.");
        if (vehicle.Status != VehicleStatus.Available)
            return Result.Fail($"El vehículo {vehicle.Plate} no está disponible.");

        driver.Status  = DriverStatus.InService;
        vehicle.Status = VehicleStatus.InService;

        service.DriverId  = driverId;
        service.VehicleId = vehicleId;

        await _driverRepo.UpdateAsync(driver);
        await _vehicleRepo.UpdateAsync(vehicle);
        await _serviceRepo.UpdateAsync(service);

        return Result.Ok($"Recursos asignados: conductor {driver.FullName}, vehículo {vehicle.Plate}.");
    }

    public async Task<Result> StartServiceAsync(int serviceId)
    {
        var service = await _serviceRepo.GetByIdAsync(serviceId);
        if (service is null)
            return Result.Fail($"No existe un servicio con ID {serviceId}.");

        if (!service.HasFullAssignment)
            return Result.Fail("El servicio no tiene conductor y vehículo asignados.");

        if (service.Status != ServiceStatus.Pending)
            return Result.Fail("Solo se pueden iniciar servicios pendientes.");

        service.Status = ServiceStatus.InProgress;
        await _serviceRepo.UpdateAsync(service);

        return Result.Ok($"Servicio {serviceId} iniciado correctamente.");
    }

    public async Task<Result> FinishServiceAsync(int serviceId)
    {
        var service = await _serviceRepo.GetWithDetailsAsync(serviceId);
        if (service is null)
            return Result.Fail($"No existe un servicio con ID {serviceId}.");

        if (service.Status != ServiceStatus.InProgress)
            return Result.Fail("Solo se pueden finalizar servicios en curso.");

        service.TotalCost = CalculateCost(service);

        if (service.Driver is not null)
        {
            service.Driver.Status = DriverStatus.Available;
            await _driverRepo.UpdateAsync(service.Driver);
        }

        if (service.Vehicle is not null)
        {
            service.Vehicle.Status = VehicleStatus.Available;
            await _vehicleRepo.UpdateAsync(service.Vehicle);
        }

        service.Status = ServiceStatus.Finished;
        await _serviceRepo.UpdateAsync(service);

        return Result.Ok($"Servicio {serviceId} finalizado. Costo: ${service.TotalCost:N0}");
    }

    private static decimal CalculateCost(TransportService service)
    {
        if (service.Vehicle is null) return 0;

        var baseCost  = service.Vehicle.RatePerKm * (decimal)service.DistanceKm;
        var surcharge = service.DistanceKm > LongDistanceThreshold
            ? SurchargePerKmLong * (decimal)service.DistanceKm
            :
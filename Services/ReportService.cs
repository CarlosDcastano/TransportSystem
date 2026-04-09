using TransportSystem.Interfaces;
using TransportSystem.Models;

namespace TransportSystem.Services;

public class ReportService
{
    private readonly ITransportServiceRepository _serviceRepo;
    private readonly IDriverRepository           _driverRepo;
    private readonly IVehicleRepository          _vehicleRepo;

    public ReportService(
        ITransportServiceRepository serviceRepo,
        IDriverRepository driverRepo,
        IVehicleRepository vehicleRepo)
    {
        _serviceRepo = serviceRepo;
        _driverRepo  = driverRepo;
        _vehicleRepo = vehicleRepo;
    }

    public async Task PrintReportAsync()
    {
        var services = (await _serviceRepo.GetAllWithDetailsAsync()).ToList();
        var drivers  = (await _driverRepo.GetAllAsync()).ToList();
        var vehicles = (await _vehicleRepo.GetAllAsync()).ToList();

        var totalFinished = services.Count(s => s.Status == ServiceStatus.Finished);
        var totalRevenue  = services.Where(s => s.Status == ServiceStatus.Finished)
                                    .Sum(s => s.TotalCost);
        var inProgress    = services.Count(s => s.Status == ServiceStatus.InProgress);
        var pending       = services.Count(s => s.Status == ServiceStatus.Pending);

        var availableDrivers  = drivers.Count(d => d.Status == DriverStatus.Available);
        var busyDrivers       = drivers.Count(d => d.Status == DriverStatus.InService);
        var availableVehicles = vehicles.Count(v => v.Status == VehicleStatus.Available);
        var busyVehicles      = vehicles.Count(v => v.Status == VehicleStatus.InService);

        Console.WriteLine();
        Console.WriteLine("========== REPORTE OPERATIVO ==========");
        Console.WriteLine($"  Servicios finalizados : {totalFinished}");
        Console.WriteLine($"  Servicios en curso    : {inProgress}");
        Console.WriteLine($"  Servicios pendientes  : {pending}");
        Console.WriteLine($"  Total ingresos        : ${totalRevenue:N0}");
        Console.WriteLine("--- Recursos ---");
        Console.WriteLine($"  Conductores disponibles : {availableDrivers} / {drivers.Count}");
        Console.WriteLine($"  Conductores en servicio : {busyDrivers}");
        Console.WriteLine($"  Vehículos disponibles   : {availableVehicles} / {vehicles.Count}");
        Console.WriteLine($"  Vehículos en servicio   : {busyVehicles}");
        Console.WriteLine("========================================");
    }
}
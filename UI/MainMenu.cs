using TransportSystem.Models;
using TransportSystem.Services;

namespace TransportSystem.UI;

public class MainMenu
{
    private readonly DriverService             _driverService;
    private readonly VehicleService            _vehicleService;
    private readonly TransportOperationService _operationService;
    private readonly ReportService             _reportService;

    public MainMenu(
        DriverService driverService,
        VehicleService vehicleService,
        TransportOperationService operationService,
        ReportService reportService)
    {
        _driverService    = driverService;
        _vehicleService   = vehicleService;
        _operationService = operationService;
        _reportService    = reportService;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("   Sistema de Gestión de Transporte     ");
            Console.WriteLine("========================================");
            Console.WriteLine("  1. Registrar conductor");
            Console.WriteLine("  2. Registrar vehículo");
            Console.WriteLine("  3. Registrar servicio de transporte");
            Console.WriteLine("  4. Asignar conductor y vehículo a servicio");
            Console.WriteLine("  5. Iniciar servicio");
            Console.WriteLine("  6. Finalizar servicio");
            Console.WriteLine("  7. Consultar servicios");
            Console.WriteLine("  8. Consultar conductores y vehículos");
            Console.WriteLine("  9. Reportes operativos");
            Console.WriteLine("  10. Salir");
            Console.WriteLine("========================================");

            var option = ConsoleHelper.ReadInt("Seleccione una opción: ");

            switch (option)
            {
                case 1:  await RegisterDriverAsync();    break;
                case 2:  await RegisterVehicleAsync();   break;
                case 3:  await RegisterServiceAsync();   break;
                case 4:  await AssignResourcesAsync();   break;
                case 5:  await StartServiceAsync();      break;
                case 6:  await FinishServiceAsync();     break;
                case 7:  await ShowServicesAsync();      break;
                case 8:  await ShowResourcesAsync();     break;
                case 9:  await ShowReportAsync();        break;
                case 10: return;
                default: ConsoleHelper.ShowError("Opción inválida."); break;
            }

            ConsoleHelper.Pause();
        }
    }

    private async Task RegisterDriverAsync()
    {
        ConsoleHelper.ShowTitle("Registrar Conductor");
        var identification = ConsoleHelper.ReadRequiredString("Identificación : ");
        var fullName       = ConsoleHelper.ReadRequiredString("Nombre completo: ");
        var license        = ConsoleHelper.ReadRequiredString("Licencia       : ");

        var result = await _driverService.RegisterDriverAsync(identification, fullName, license);

        if (result.Success) ConsoleHelper.ShowSuccess(result.Message);
        else                ConsoleHelper.ShowError(result.Message);
    }

    private async Task RegisterVehicleAsync()
    {
        ConsoleHelper.ShowTitle("Registrar Vehículo");
        var plate = ConsoleHelper.ReadRequiredString("Placa: ");

        Console.WriteLine("Tipos: 0=Car, 1=Motorcycle, 2=Truck, 3=Van, 4=Bus");
        var typeInt = ConsoleHelper.ReadInt("Tipo de vehículo: ");
        var type    = (VehicleType)typeInt;

        var capacity = ConsoleHelper.ReadInt("Capacidad: ");

        var result = await _vehicleService.RegisterVehicleAsync(plate, type, capacity);

        if (result.Success) ConsoleHelper.ShowSuccess(result.Message);
        else                ConsoleHelper.ShowError(result.Message);
    }

    private async Task RegisterServiceAsync()
    {
        ConsoleHelper.ShowTitle("Registrar Servicio");
        var origin      = ConsoleHelper.ReadRequiredString("Origen     : ");
        var destination = ConsoleHelper.ReadRequiredString("Destino    : ");
        var distance    = ConsoleHelper.ReadDouble("Distancia (km): ");

        var result = await _operationService.RegisterServiceAsync(origin, destination, distance);

        if (result.Success) ConsoleHelper.ShowSuccess(result.Message);
        else                ConsoleHelper.ShowError(result.Message);
    }

    private async Task AssignResourcesAsync()
    {
        ConsoleHelper.ShowTitle("Asignar Recursos");
        var serviceId = ConsoleHelper.ReadInt("ID del servicio : ");
        var driverId  = ConsoleHelper.ReadInt("ID del conductor: ");
        var vehicleId = ConsoleHelper.ReadInt("ID del vehículo : ");

        var result = await _operationService.AssignResourcesAsync(serviceId, driverId, vehicleId);

        if (result.Success) ConsoleHelper.ShowSuccess(result.Message);
        else                ConsoleHelper.ShowError(result.Message);
    }

    private async Task StartServiceAsync()
    {
        ConsoleHelper.ShowTitle("Iniciar Servicio");
        var serviceId = ConsoleHelper.ReadInt("ID del servicio: ");

        var result = await _operationService.StartServiceAsync(serviceId);

        if (result.Success) ConsoleHelper.ShowSuccess(result.Message);
        else                ConsoleHelper.ShowError(result.Message);
    }

    private async Task FinishServiceAsync()
    {
        ConsoleHelper.ShowTitle("Finalizar Servicio");
        var serviceId = ConsoleHelper.ReadInt("ID del servicio: ");

        var result = await _operationService.FinishServiceAsync(serviceId);

        if (result.Success) ConsoleHelper.ShowSuccess(result.Message);
        else                ConsoleHelper.ShowError(result.Message);
    }

    private async Task ShowServicesAsync()
    {
        ConsoleHelper.ShowTitle("Consultar Servicios");
        var services = (await _operationService.GetAllServicesAsync()).ToList();

        if (!services.Any())
        {
            ConsoleHelper.ShowError("No hay servicios registrados.");
            return;
        }

        foreach (var s in services)
        {
            var driver  = s.Driver?.FullName  ?? "Sin asignar";
            var vehicle = s.Vehicle?.Plate    ?? "Sin asignar";
            Console.WriteLine($"\n  [ID:{s.Id}] {s.Origin} → {s.Destination}");
            Console.WriteLine($"  Distancia : {s.DistanceKm} km");
            Console.WriteLine($"  Estado    : {s.Status}");
            Console.WriteLine($"  Conductor : {driver}");
            Console.WriteLine($"  Vehículo  : {vehicle}");
            Console.WriteLine($"  Costo     : ${s.TotalCost:N0}");
            Console.WriteLine("  ----------------------------------------");
        }
    }

    private async Task ShowResourcesAsync()
    {
        ConsoleHelper.ShowTitle("Conductores");
        var drivers = (await _driverService.GetAllAsync()).ToList();

        if (!drivers.Any())
            ConsoleHelper.ShowError("No hay conductores registrados.");
        else
            foreach (var d in drivers)
                Console.WriteLine($"  [ID:{d.Id}] {d.FullName} | {d.Identification} | {d.License} | {d.Status}");

        ConsoleHelper.ShowTitle("Vehículos");
        var vehicles = (await _vehicleService.GetAllAsync()).ToList();

        if (!vehicles.Any())
            ConsoleHelper.ShowError("No hay vehículos registrados.");
        else
            foreach (var v in vehicles)
                Console.WriteLine($"  [ID:{v.Id}] {v.Plate} | {v.Type} | Capacidad: {v.Capacity} | {v.Status}");
    }

    private async Task ShowReportAsync()
    {
        await _reportService.PrintReportAsync();
    }
}
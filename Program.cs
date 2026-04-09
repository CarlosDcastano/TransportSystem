using TransportSystem.Data;
using TransportSystem.Repositories;
using TransportSystem.Services;
using TransportSystem.UI;

var context           = new TransportDbContext();
var driverRepo        = new DriverRepository(context);
var vehicleRepo       = new VehicleRepository(context);
var serviceRepo       = new TransportServiceRepository(context);

var driverService     = new DriverService(driverRepo);
var vehicleService    = new VehicleService(vehicleRepo);
var operationService  = new TransportOperationService(serviceRepo, driverRepo, vehicleRepo);
var reportService     = new ReportService(serviceRepo, driverRepo, vehicleRepo);

var menu = new MainMenu(driverService, vehicleService, operationService, reportService);

await context.Database.EnsureCreatedAsync();
await menu.RunAsync();
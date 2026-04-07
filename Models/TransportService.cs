namespace TransportSystem.Models;

public enum ServiceStatus
{
    Pending,
    InProgress,
    Finished
}

public class TransportService
{
    public int Id { get; set; }
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public double DistanceKm { get; set; }
    public ServiceStatus Status { get; set; } = ServiceStatus.Pending;
    public decimal TotalCost { get; set; } = 0;

    public int? DriverId { get; set; }
    public int? VehicleId { get; set; }
}
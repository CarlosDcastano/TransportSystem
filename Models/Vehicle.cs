namespace TransportSystem.Models;

public enum VehicleStatus
{
    Available,
    InService,
    OutOfOperation
}

public enum VehicleType
{
    Car,
    Motorcycle,
    Truck,
    Van,
    Bus
}

public class Vehicle
{
    public int Id { get; set; }
    public string Plate { get; set; } = string.Empty;
    public VehicleType Type { get; set; }
    public int Capacity { get; set; }
    public VehicleStatus Status { get; set; } = VehicleStatus.Available;
    
    public decimal RatePerKm => Type switch
    {
        VehicleType.Motorcycle => 800m,
        VehicleType.Car        => 1200m,
        VehicleType.Van        => 1800m,
        VehicleType.Bus        => 2500m,
        VehicleType.Truck      => 3200m,
        _                      => 1000m
    };
}
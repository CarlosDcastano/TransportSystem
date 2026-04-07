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
}
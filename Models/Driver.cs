namespace TransportSystem.Models;

public enum DriverStatus
{
    Available,
    InService,
    Inactive
}

public class Driver
{
    public int Id { get; set; }
    public string Identification { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string License { get; set; } = string.Empty;
    public DriverStatus Status { get; set; } = DriverStatus.Available;
}



namespace TransportSystem.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }
    
}
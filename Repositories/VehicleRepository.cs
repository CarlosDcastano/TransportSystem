using Microsoft.EntityFrameworkCore;
using TransportSystem.Data;
using TransportSystem.Interfaces;
using TransportSystem.Models;

namespace TransportSystem.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly TransportDbContext _context;

    public VehicleRepository(TransportDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync() =>
        await _context.Vehicles.ToListAsync();

    public async Task<Vehicle?> GetByIdAsync(int id) =>
        await _context.Vehicles.FindAsync(id);

    public async Task<Vehicle?> GetByPlateAsync(string plate) =>
        await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Plate == plate);

    public async Task<IEnumerable<Vehicle>> GetAvailableAsync() =>
        await _context.Vehicles
            .Where(v => v.Status == VehicleStatus.Available)
            .ToListAsync();

    public async Task AddAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vehicle vehicle)
    {
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id) =>
        await _context.Vehicles.AnyAsync(v => v.Id == id);
}
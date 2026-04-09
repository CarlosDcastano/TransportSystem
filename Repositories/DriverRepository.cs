using Microsoft.EntityFrameworkCore;
using TransportSystem.Data;
using TransportSystem.Interfaces;
using TransportSystem.Models;

namespace TransportSystem.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly TransportDbContext _context;

    public DriverRepository(TransportDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Driver>> GetAllAsync() =>
        await _context.Drivers.ToListAsync();

    public async Task<Driver?> GetByIdAsync(int id) =>
        await _context.Drivers.FindAsync(id);

    public async Task<Driver?> GetByIdentificationAsync(string identification) =>
        await _context.Drivers
            .FirstOrDefaultAsync(d => d.Identification == identification);

    public async Task<IEnumerable<Driver>> GetAvailableAsync() =>
        await _context.Drivers
            .Where(d => d.Status == DriverStatus.Available)
            .ToListAsync();

    public async Task AddAsync(Driver driver)
    {
        await _context.Drivers.AddAsync(driver);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Driver driver)
    {
        _context.Drivers.Update(driver);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id) =>
        await _context.Drivers.AnyAsync(d => d.Id == id);
}
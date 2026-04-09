using Microsoft.EntityFrameworkCore;
using TransportSystem.Data;
using TransportSystem.Interfaces;
using TransportSystem.Models;

namespace TransportSystem.Repositories;

public class TransportServiceRepository : ITransportServiceRepository
{
    private readonly TransportDbContext _context;

    public TransportServiceRepository(TransportDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TransportService>> GetAllAsync() =>
        await _context.TransportServices.ToListAsync();

    public async Task<IEnumerable<TransportService>> GetAllWithDetailsAsync() =>
        await _context.TransportServices
            .Include(s => s.Driver)
            .Include(s => s.Vehicle)
            .ToListAsync();

    public async Task<TransportService?> GetByIdAsync(int id) =>
        await _context.TransportServices.FindAsync(id);

    public async Task<TransportService?> GetWithDetailsAsync(int id) =>
        await _context.TransportServices
            .Include(s => s.Driver)
            .Include(s => s.Vehicle)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<IEnumerable<TransportService>> GetByStatusAsync(ServiceStatus status) =>
        await _context.TransportServices
            .Include(s => s.Driver)
            .Include(s => s.Vehicle)
            .Where(s => s.Status == status)
            .ToListAsync();

    public async Task AddAsync(TransportService service)
    {
        await _context.TransportServices.AddAsync(service);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TransportService service)
    {
        _context.TransportServices.Update(service);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id) =>
        await _context.TransportServices.AnyAsync(s => s.Id == id);
    
    
}
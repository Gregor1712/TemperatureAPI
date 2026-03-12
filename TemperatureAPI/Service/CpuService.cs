using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemperatureAPI.Dbo;
using TemperatureAPI.Interfaces;

namespace TemperatureAPI.Service;

public class CpuService : ICpuService
{
    private readonly ApplicationDbContext _context;

    public CpuService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<CpuDBO>> GetCPU()
    {
        return await _context.CPU
            .Include(c => c.Manufacturer)
            .ToListAsync();
    }
}
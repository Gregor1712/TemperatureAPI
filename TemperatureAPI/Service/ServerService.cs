using AutoMapper;
using TemperatureAPI.DTO;
using TemperatureAPI.Interfaces;

namespace TemperatureAPI.Service;

public class ServerService : IServerService
{
    private readonly ApplicationDbContext _context;
    private readonly ICpuService _cpuService;
    private readonly IMapper _mapper;
    
    public ServerService(
        ApplicationDbContext context,
        ICpuService cpuService,
        IMapper mapper)
    {
        _context = context;
        _cpuService = cpuService;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<CpuDTO>> GetCPU()
    {
        var cpu = await _cpuService.GetCPU();
        var cpuToReturn = _mapper.Map<IEnumerable<CpuDTO>>(cpu);
        return cpuToReturn;
    }
}
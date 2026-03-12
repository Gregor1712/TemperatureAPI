using TemperatureAPI.DTO;

namespace TemperatureAPI.Interfaces;

public interface IServerService
{
    public Task<IEnumerable<CpuDTO>> GetCPU();
}
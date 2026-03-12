using Microsoft.AspNetCore.Mvc;
using TemperatureAPI.Dbo;

namespace TemperatureAPI.Interfaces;

public interface ICpuService
{
    public Task<IEnumerable<CpuDBO>> GetCPU();   
}
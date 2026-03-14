using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TemperatureAPI.DTO;
using TemperatureAPI.Interfaces;
using TemperatureAPI.Models;
using TemperatureAPI.RequestHelpers;
using TemperatureAPI.Specifications;

namespace TemperatureAPI.Controllers;

public class TemperatureHistoryController(IUnitOfWork unit, IMapper mapper) : BaseApiController(mapper)
{
    [Authorize(Policy = "RequireUserRole")]
    [HttpGet]
    public async Task<ActionResult<Pagination<TemperatureHistoryDto>>> GetTemperatureHistory([FromQuery] TemperatureHistorySpecParams historyParams)
    {
        var spec = new TemperatureHistorySpecification(historyParams);
        return await CreatePagedResultDto<TemperatureHistory, TemperatureHistoryDto>(
            unit.Repository<TemperatureHistory>(), spec, historyParams.PageIndex, historyParams.PageSize);
    }
}
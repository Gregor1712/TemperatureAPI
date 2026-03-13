using AutoMapper;
using TemperatureAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TemperatureAPI.Entities;
using TemperatureAPI.RequestHelpers;

namespace TemperatureAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    public readonly IMapper? _mapper;

    protected BaseApiController()
    { }
    
    protected BaseApiController(IMapper mapper)
    {
        _mapper = mapper;
    }

    protected async Task<ActionResult> CreatePagedResultDto<T, TDto>(
        IGenericRepository<T> repository,
        ISpecification<T> specification,
        int pageIndex,
        int pageSize)
        where T : BaseEntity
        where TDto : class
    {
        var items = await repository.ListAsync(specification);
        var totalItems = await repository.CountAsync(specification);
        var data = _mapper?.Map<IReadOnlyList<TDto>>(items);
        if (data != null)
        {
            var pagination = new Pagination<TDto>(pageIndex, pageSize, totalItems, data);
            return Ok(pagination);
        }
        return NoContent();
    }
}
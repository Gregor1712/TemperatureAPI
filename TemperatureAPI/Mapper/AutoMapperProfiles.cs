using AutoMapper;
using TemperatureAPI.Mapper;
using TemperatureAPI.DTO;
using TemperatureAPI.Models;

namespace TemperatureAPI.Mapper;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<TemperatureHistory, TemperatureHistoryDto>();
        CreateMap<TemperatureHistoryDto, TemperatureHistory>();
    }
}
using TemperatureAPI.Models;

namespace TemperatureAPI.Specifications;

public class TemperatureHistorySpecification : BaseSpecification<TemperatureHistory>
{
    public TemperatureHistorySpecification(TemperatureHistorySpecParams histortyParams)
        : base(x =>
            (string.IsNullOrEmpty(histortyParams.City) || x.City.ToLower().Contains(histortyParams.City)) &&
            (!histortyParams.TemperatureC.HasValue || x.TemperatureC == histortyParams.TemperatureC))

    {
        ApplyPaging(histortyParams.PageSize * (histortyParams.PageIndex - 1), histortyParams.PageSize);

        switch (histortyParams.Sort)
        {
            case "cityAsc":
                AddOrderBy(x => x.City);
                break;
            case "cityDesc":
                AddOrderByDescending(x => x.City);
                break;
            case "temperatureAsc":
                AddOrderBy(x => x.TemperatureC);
                break;
            case "temperatureDesc":
                AddOrderByDescending(x => x.TemperatureC);
                break;
            case "measuredAtUtcAsc":
                AddOrderBy(x => x.MeasuredAtUtc);
                break;
            case "measuredAtUtcDesc":
                AddOrderByDescending(x => x.MeasuredAtUtc);
                break;
            default:
                AddOrderBy(x => x.City);
                break;
        }
    }
}
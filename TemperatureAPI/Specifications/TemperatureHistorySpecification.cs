using System.Globalization;
using TemperatureAPI.Models;

namespace TemperatureAPI.Specifications;

public class TemperatureHistorySpecification : BaseSpecification<TemperatureHistory>
{
    public TemperatureHistorySpecification(TemperatureHistorySpecParams historyParams)
        : base(x =>
            (string.IsNullOrEmpty(historyParams.City) || x.City.ToLower().Contains(historyParams.City)) &&
            (!historyParams.TemperatureC.HasValue || x.TemperatureC.ToString().Contains(historyParams.TemperatureC.Value.ToString())))

    {
        ApplyPaging(historyParams.PageSize * (historyParams.PageIndex - 1), historyParams.PageSize);

        switch (historyParams.Sort)
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
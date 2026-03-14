using FluentAssertions;
using TemperatureAPI.Models;

namespace TemperatureAPI.Tests;

public class CityMappingTests
{
    [Theory]
    [InlineData("bratislava", 1)]
    [InlineData("praha", 2)]
    [InlineData("budapest", 3)]
    [InlineData("vieden", 4)]
    public void TryGetCityId_SupportedCity_ReturnsTrueAndCorrectId(string city, int expectedId)
    {
        var result = CityMapping.TryGetCityId(city, out var cityId);

        result.Should().BeTrue();
        cityId.Should().Be(expectedId);
    }

    [Theory]
    [InlineData("Bratislava")]
    [InlineData("PRAHA")]
    [InlineData("BuDaPeSt")]
    public void TryGetCityId_CaseInsensitive_ReturnsTrue(string city)
    {
        var result = CityMapping.TryGetCityId(city, out _);

        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("london")]
    [InlineData("")]
    [InlineData("newyork")]
    public void TryGetCityId_UnsupportedCity_ReturnsFalse(string city)
    {
        var result = CityMapping.TryGetCityId(city, out var cityId);

        result.Should().BeFalse();
        cityId.Should().Be(0);
    }

    [Fact]
    public void SupportedCities_ContainsAllFourCities()
    {
        CityMapping.SupportedCities.Should().HaveCount(4);
        CityMapping.SupportedCities.Should().Contain(new[] { "bratislava", "praha", "budapest", "vieden" });
    }
}
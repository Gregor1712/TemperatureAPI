using System.Security.Claims;
using FluentAssertions;
using TemperatureAPI.Extensions;

namespace TemperatureAPI.Tests;

public class ClaimsPrincipalExtensionsTests
{
    [Fact]
    public void GetMemberId_WithValidClaim_ReturnsId()
    {
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "user-123") };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);

        var result = principal.GetMemberId();

        result.Should().Be("user-123");
    }

    [Fact]
    public void GetMemberId_WithoutClaim_ThrowsException()
    {
        var identity = new ClaimsIdentity();
        var principal = new ClaimsPrincipal(identity);

        var act = () => principal.GetMemberId();

        act.Should().Throw<Exception>().WithMessage("*memberId*");
    }
}
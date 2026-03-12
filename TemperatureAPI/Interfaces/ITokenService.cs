using TemperatureAPI.Entities;

namespace TemperatureAPI.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
    string GenerateRefreshToken();
}
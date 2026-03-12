using TemperatureAPI.DTO;
using TemperatureAPI.Entities;
using TemperatureAPI.Interfaces;

namespace TemperatureAPI.Extensions;

public static class AppUserExtensions
{
    public static async Task<UserDto> ToDto(this AppUser user, ITokenService tokenService)
    {
        return new UserDto
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email!,
            Token = await tokenService.CreateToken(user)
        };
    }
}

namespace TemperatureAPI.Interfaces;

public interface IUserDataSeeder
{
    Task SeedUsers();
    string HashPassword(string password);
}
namespace TemperatureAPI.Interfaces;

public interface ICsvDataSeeder
{
    public Task SeedDataAsync();
    public Task SeedManufacturersAsync();
    public Task SeedCPUsAsync();
}
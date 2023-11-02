namespace DistanceCalculatorApi.Application.Interfaces;

public interface ILocationProviderService
{
    Task<string?> GetCountryCodeAsync(string ipAddress, CancellationToken cancellationToken = default);
    
}
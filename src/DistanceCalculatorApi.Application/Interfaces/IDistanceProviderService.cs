namespace DistanceCalculatorApi.Application.Interfaces;

using DTOs;
using Enums;

public interface IDistanceProviderService
{
    Task<DistanceResponse> GetDistanceAsync(CoordinateDto pointA, CoordinateDto pointB, string ipAddress,
        CalculationType type,
        Unit? unit = null,
        CancellationToken cancellationToken = default);
}
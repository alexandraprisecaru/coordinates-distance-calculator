namespace DistanceCalculatorAPI.Shared.Services;

using System.Globalization;
using DistanceCalculatorApi.Application.DTOs;
using DistanceCalculatorApi.Application.Enums;
using DistanceCalculatorApi.Application.Interfaces;
using Serilog;

public class DistanceProviderService : IDistanceProviderService
{
    private readonly IDistanceCalculatorService _distanceCalculatorService;
    private readonly ILocationProviderService _locationProviderService;
    private readonly ILogger _log;

    public DistanceProviderService(IDistanceCalculatorService distanceCalculatorService,
        ILocationProviderService locationProviderService, ILogger log)
    {
        _distanceCalculatorService = distanceCalculatorService;
        _locationProviderService = locationProviderService;
        _log = log;
    }

    public async Task<DistanceResponse> GetDistanceAsync(CoordinateDto pointA, CoordinateDto pointB, string ipAddress,
        CalculationType type,
        Unit? unit = null,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        unit ??= await GetUnitByIpAddress(ipAddress, cancellationToken);

        this._log.Information("Calculating distance between coordinates on sphere");
        var distance = this._distanceCalculatorService.CalculateFlatDistance(pointA, pointB);

        switch (type)
        {
            case CalculationType.Spherical:
                distance = this._distanceCalculatorService.CalculateSphericalDistance(pointA, pointB);
                break;
            case CalculationType.Flat:
                distance = this._distanceCalculatorService.CalculateFlatDistance(pointA, pointB);
                break;
            case CalculationType.Ellipsoidal:
                distance = this._distanceCalculatorService.CalculateEllipsoidalDistance(pointA, pointB);
                break;
        }

        if (unit is Unit.Imperial)
            distance *= 0.621371;

        return new DistanceResponse(distance, unit);
    }

    private async Task<Unit> GetUnitByIpAddress(string ipAddress, CancellationToken cancellationToken = default)
    {
        this._log.Information("Attempting to get country based on IP");
        cancellationToken.ThrowIfCancellationRequested();

        var country = await this._locationProviderService.GetCountryCodeAsync(ipAddress, cancellationToken);
        if (country is null)
        {
            this._log.Warning("Couldn't identify country based on ip address");
            return Unit.Metric;
        }

        this._log.Information("Getting unit based on country");
        return new RegionInfo(country).IsMetric ? Unit.Metric : Unit.Imperial;
    }
}
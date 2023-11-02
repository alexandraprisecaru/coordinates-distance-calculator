namespace DistanceCalculatorAPI.IntegrationTests;

using DistanceCalculatorApi.Application.DTOs;
using DistanceCalculatorApi.Application.Enums;
using DistanceCalculatorApi.Application.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Shared.Services;

public class DistanceCalculatorTests : IClassFixture<SharedFixture>
{
    private readonly DistanceProviderService _distanceProviderService;

    private readonly CoordinateDto _pointA;
    private readonly CoordinateDto _pointB;
    private readonly Unit _unit;

    const string USAIpAddress = "101.33.20.0";
    const string ROAIpAddress = "109.163.226.0";
    
    public DistanceCalculatorTests(SharedFixture fixture)
    {
        ServiceProvider serviceProvider
            = fixture.ServiceProvider;

        IDistanceCalculatorService distanceCalculatorService = serviceProvider.GetService<IDistanceCalculatorService>()!;
        ILocationProviderService locationProviderService = serviceProvider.GetService<ILocationProviderService>()!;
        ILogger logger
            = serviceProvider.GetService<ILogger>()!;
        
        _distanceProviderService = new DistanceProviderService(distanceCalculatorService, locationProviderService, logger);

        _pointA = new CoordinateDto(12.456, 67.890);
        _pointB = new CoordinateDto(88.789, 100.678);
        _unit = Unit.Metric;
    }

    [Fact]
    public async Task ShouldGetDistanceBasedOnUnitProvided()
    {
        var result =
            await _distanceProviderService.GetDistanceAsync(_pointA, _pointB, ROAIpAddress, _unit);

        result.Should().NotBeNull();
        result.Unit.Should().Be(_unit);
        result.Distance.Should().Be(10239.52659935647);
    }
    
    [Fact]
    public async Task ShouldIdentifyMetricUnitBasedOnIpAndUseIdtToGetDistance()
    {
        var result =
            await _distanceProviderService.GetDistanceAsync(_pointA, _pointB, ROAIpAddress);

        result.Should().NotBeNull();
        result.Unit.Should().Be(Unit.Metric);
        result.Distance.Should().Be(10239.52659935647);
    }
    
    [Fact]
    public async Task ShouldIdentifyImperialUnitBasedOnIpAndUseIdtToGetDistance()
    {
        var result =
            await _distanceProviderService.GetDistanceAsync(_pointA, _pointB, USAIpAddress);

        result.Should().NotBeNull();
        result.Unit.Should().Be(Unit.Imperial);
        result.Distance.Should().Be(6362.544882568729);
    }
}
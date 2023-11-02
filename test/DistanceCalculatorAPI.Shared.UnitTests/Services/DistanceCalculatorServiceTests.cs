namespace DistanceCalculatorAPI.Shared.UnitTests.Services;

using DistanceCalculatorApi.Application.DTOs;
using DistanceCalculatorApi.Application.Enums;
using FluentAssertions;
using Shared.Services;
using Xunit;

public class DistanceCalculatorServiceTests
{
    private readonly DistanceCalculatorService _distanceCalculatorService;
    private readonly CoordinateDto _firstCoordinate;
    private readonly CoordinateDto _secondCoordinate;

    public DistanceCalculatorServiceTests()
    {
        _firstCoordinate = new CoordinateDto(53.297975, -6.372663);
        _secondCoordinate = new CoordinateDto(41.385101, -81.440440);
        
        _distanceCalculatorService = new DistanceCalculatorService();
    }

    [Fact]
    public void CalculateDistanceShouldReturnMetricDistanceInKm()
    {
        // Act
        var distance = _distanceCalculatorService.CalculateDistance(_firstCoordinate, _secondCoordinate, Unit.Metric);

        // Assert
        distance.Should().Be(4617.36419640607);
    }
    
    [Fact]
    public void CalculateDistanceShouldReturnDistanceInMiles()
    {
        // Act
        var distance = _distanceCalculatorService.CalculateDistance(_firstCoordinate, _secondCoordinate, Unit.Imperial);

        // Assert
        distance.Should().Be(2869.096208085036);
    }
}
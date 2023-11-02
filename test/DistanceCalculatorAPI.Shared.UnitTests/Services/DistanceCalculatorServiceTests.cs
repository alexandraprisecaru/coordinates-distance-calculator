namespace DistanceCalculatorAPI.Shared.UnitTests.Services;

using DistanceCalculatorApi.Application.DTOs;
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
    public void CalculateSphericalDistanceShouldReturnResult()
    {
        // Act
        var distance = _distanceCalculatorService.CalculateSphericalDistance(_firstCoordinate, _secondCoordinate);

        // Assert
        distance.Should().Be(5536.338682266685);
    }
    
    [Fact]
    public void CalculateFlatDistanceShouldReturnResult()
    {
        // Act
        var distance = _distanceCalculatorService.CalculateFlatDistance(_firstCoordinate, _secondCoordinate);

        // Assert
        distance.Should().Be(8456.26940135452);
    }
    
    [Fact]
    public void CalculateEllipsoidalDistanceShouldReturnResult()
    {
        // Act
        var distance = _distanceCalculatorService.CalculateEllipsoidalDistance(_firstCoordinate, _secondCoordinate);

        // Assert
        distance.Should().Be(5541.118130195186);
    }

}
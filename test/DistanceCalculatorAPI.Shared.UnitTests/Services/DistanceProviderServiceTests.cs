namespace DistanceCalculatorAPI.Shared.UnitTests.Services;

using AutoFixture;
using DistanceCalculatorApi.Application.DTOs;
using DistanceCalculatorApi.Application.Enums;
using DistanceCalculatorApi.Application.Interfaces;
using FluentAssertions;
using Moq;
using Serilog;
using Shared.Services;
using Xunit;

public class DistanceProviderServiceTests
{
    private readonly string _ipAddress;

    private readonly CoordinateDto _pointA;
    private readonly CoordinateDto _pointB;
    private readonly Unit _unit;
    private readonly Mock<IDistanceCalculatorService> _mockDistanceCalculatorService;
    private readonly Mock<ILocationProviderService> _mockLocationProviderService;
    
    private readonly DistanceProviderService _distanceProviderService;

    public DistanceProviderServiceTests()
    {
        var fixture = new Fixture();

        _pointA = fixture.Create<CoordinateDto>();
        _pointB = fixture.Create<CoordinateDto>();
        _ipAddress = fixture.Create<string>();
        _unit = Unit.Metric;

        _mockDistanceCalculatorService = new Mock<IDistanceCalculatorService>();
        _mockLocationProviderService = new Mock<ILocationProviderService>();

        var mockLogger = new Mock<ILogger>();

        _distanceProviderService = new DistanceProviderService(_mockDistanceCalculatorService.Object,
            _mockLocationProviderService.Object, mockLogger.Object);
    }

    [Fact]
    public void GetDistanceAsyncShouldThrowExceptionOnCancel()
    {
        // Arrange
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        // Act + Assert
        var call = async () =>
            await _distanceProviderService.GetDistanceAsync(_pointA, _pointB, _ipAddress, _unit,
                cancellationTokenSource.Token);
        call.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task GetDistanceAsyncShouldGetUnitBasedOnIpAddressIfUnitIsMissing()
    {
        // Arrange
        _mockLocationProviderService
            .Setup(x => x.GetCountryCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("US");

        var expectedDistance = 30.56;
        Unit? unit = null;
        
        _mockDistanceCalculatorService
            .Setup(x => x.CalculateDistance(It.IsAny<CoordinateDto>(), It.IsAny<CoordinateDto>(),It.IsAny<Unit>()))
            .Returns(expectedDistance);

        // Act
        var response = await _distanceProviderService.GetDistanceAsync(_pointA, _pointB, _ipAddress, unit);

        // Assert
        _mockLocationProviderService.Verify(
            x => x.GetCountryCodeAsync(_ipAddress, It.IsAny<CancellationToken>()), Times.Once);
        
        _mockDistanceCalculatorService.Verify(
            x => x.CalculateDistance(_pointA, _pointB, Unit.Imperial), Times.Once);

        response.Should().NotBeNull();
        response.Distance.Should().Be(expectedDistance);
        response.Unit.Should().Be(Unit.Imperial);
    }

    [Fact]
    public async Task GetDistanceAsyncShouldUseUnitIfProvided()
    {
        // Arrange
        var expectedDistance = 30.56;
        var unit = Unit.Metric;
        _mockDistanceCalculatorService
            .Setup(x => x.CalculateDistance(It.IsAny<CoordinateDto>(), It.IsAny<CoordinateDto>(),It.IsAny<Unit>()))
            .Returns(expectedDistance);

        // Act
        var response = await _distanceProviderService.GetDistanceAsync(_pointA, _pointB, _ipAddress, unit);

        // Assert
        _mockLocationProviderService.Verify(
            x => x.GetCountryCodeAsync(_ipAddress, It.IsAny<CancellationToken>()), Times.Never);
        
        _mockDistanceCalculatorService.Verify(
            x => x.CalculateDistance(_pointA, _pointB, unit), Times.Once);

        response.Should().NotBeNull();
        response.Distance.Should().Be(expectedDistance);
        response.Unit.Should().Be(unit);
    }
}
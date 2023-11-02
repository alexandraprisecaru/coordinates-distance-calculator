namespace DistanceCalculatorAPI.Shared.UnitTests.Services;

using AutoFixture;
using DistanceCalculatorApi.Application.Configurations;
using DistanceCalculatorApi.Application.DTOs;
using FluentAssertions;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;
using Flurl.Http.Testing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using Serilog;
using Shared.Services;
using Xunit;

public class LocationProviderServiceTests
{
    private readonly Fixture _fixture;

    private readonly string _ipAddress;
    private readonly HttpTest _httpTest;
    
    private readonly LocationProviderService _locationProviderService;

    public LocationProviderServiceTests()
    {
        _fixture = new Fixture();

        _ipAddress = _fixture.Create<string>();
        var ipApiSettings = _fixture.Build<IpApiSettings>()
            .With(x => x.Url, _fixture.Create<Uri>().ToString().TrimEnd('/'))
            .Create();

        _httpTest = new HttpTest();
        var mockIpApiSettingsOptions = new Mock<IOptions<IpApiSettings>>();
        mockIpApiSettingsOptions.Setup(x => x.Value).Returns(ipApiSettings);

        var mockFlurlClientFactory = new Mock<IFlurlClientFactory>();
        mockFlurlClientFactory
            .Setup(x => x.Get(It.IsAny<Url>()))
            .Returns(new FlurlClient(ipApiSettings.Url));

        var mockLogger = new Mock<ILogger>();

        _locationProviderService = new LocationProviderService(mockIpApiSettingsOptions.Object,
            mockFlurlClientFactory.Object, mockLogger.Object);
    }

    [Fact]
    public void GetCountryCodeAsyncShouldThrowExceptionOnCancel()
    {
        // Arrange
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        // Act + Assert
        var call = async () => await _locationProviderService.GetCountryCodeAsync(_ipAddress, cancellationTokenSource.Token);
        call.Should().ThrowAsync<OperationCanceledException>();
    }
    
    [Fact]
    public async Task GetCountryCodeAsyncShouldReturnNullIfApiRequestFails()
    {
        // Arrange
        var expectedResponse = "Error message";
        _httpTest.RespondWith(expectedResponse, StatusCodes.Status400BadRequest);
            
        // Act
        var response = await _locationProviderService.GetCountryCodeAsync(_ipAddress);
        
        // Assert
        response.Should().BeNull();
    }
    
    [Fact]
    public async Task GetCountryCodeAsyncShouldReturnCountryCode()
    {
        // Arrange
        var expectedResponse = _fixture.Create<LocationResponse>();
        _httpTest.RespondWith(JsonConvert.SerializeObject(expectedResponse));
            
        // Act
        var response = await _locationProviderService.GetCountryCodeAsync(_ipAddress);
        
        // Assert
        response.Should().NotBeNull();
        response.Should().Be(expectedResponse.CountryCode);
    }
}
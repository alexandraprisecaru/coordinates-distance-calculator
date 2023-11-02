namespace DistanceCalculatorAPI.Shared.Services;

using DistanceCalculatorApi.Application.Configurations;
using DistanceCalculatorApi.Application.DTOs;
using DistanceCalculatorApi.Application.Interfaces;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Options;
using Serilog;

public class LocationProviderService : ILocationProviderService
{
    private readonly ILogger _log;
    private readonly IFlurlClient _client;
    private readonly IpApiSettings _ipApiSettings;

    public LocationProviderService(IOptions<IpApiSettings> ipApiSettings, IFlurlClientFactory clientFactory, ILogger log)
    {
        _ipApiSettings = ipApiSettings.Value;
        _log = log;
        _client = clientFactory.Get(_ipApiSettings.Url);
    }
    
    public async Task<string?> GetCountryCodeAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var locationInfo = await _client.Request($"{ipAddress}")
            .SetQueryParam("access_key", _ipApiSettings.Key)
            .OnError(call =>
            {
                call.ExceptionHandled = true;
                _log.Error(
                    "An error occured while trying to get country by ipAddress: {ipAddress} | {StatusCode} | {exceptionMessage}",
                    ipAddress, call.Response.StatusCode, call.Exception.Message);
            })
            .GetJsonAsync<LocationResponse>(cancellationToken);

        return locationInfo?.CountryCode;
    }
}
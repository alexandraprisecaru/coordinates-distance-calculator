namespace DistanceCalculatorAPI.Shared;

using DistanceCalculatorApi.Application.Interfaces;
using Flurl.Http.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedLayer(this IServiceCollection services)
    {
        services.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();

        services.TryAddScoped<IDistanceProviderService, DistanceProviderService>();
        services.TryAddScoped<IDistanceCalculatorService, DistanceCalculatorService>();
        services.TryAddScoped<ILocationProviderService, LocationProviderService>();

        return services;
    }
}
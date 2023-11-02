namespace DistanceCalculatorAPI.IntegrationTests;

using DistanceCalculatorApi.Application.Configurations;
using DistanceCalculatorApi.Application.Interfaces;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using Shared.Services;

public class SharedFixture
{
    public ServiceProvider ServiceProvider { get; private set; }

    public SharedFixture()
    {
        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(
                path: "appsettings.json",
                optional: false,
                reloadOnChange: true)
            .Build();
        
        services.AddSingleton<IConfiguration>(configuration);
 
        services.Configure<IpApiSettings>(configuration.GetSection("IpApiSettings"));
        
        var logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .CreateLogger();
        services.TryAddSingleton<ILogger>(logger);

        services.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();

        services.TryAddScoped<IDistanceProviderService, DistanceProviderService>();
        services.TryAddScoped<IDistanceCalculatorService, DistanceCalculatorService>();
        services.TryAddScoped<ILocationProviderService, LocationProviderService>();

        ServiceProvider = services.BuildServiceProvider();
    }
}

using DistanceCalculatorApi.Application;
using DistanceCalculatorApi.Application.Configurations;
using DistanceCalculatorApi.Application.DTOs;
using DistanceCalculatorApi.Application.Enums;
using DistanceCalculatorApi.Application.Interfaces;
using DistanceCalculatorAPI.Extensions;
using DistanceCalculatorAPI.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Serilog;

const string LocalIpV4 = "127.0.0.1";
const string LocalIpV6 = "::1";
const string ROAIpAddress = "109.163.226.0";

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<IpApiSettings>(builder.Configuration.GetSection("IpApiSettings"));

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Host.UseSerilog(logger);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationLayer();
builder.Services.AddSharedLayer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var distance = app.MapGroup("distance");
distance.MapPost("sphere", GetSphericalDistanceAsync);
distance.MapPost("flat", GetFlatDistanceAsync);
distance.MapPost("ellipsoid", GetEllipsoidalDistanceAsync);

async Task<IResult> GetSphericalDistanceAsync(IDistanceProviderService distanceProviderService,
    IValidator<GetDistanceRequest> validator, HttpContext context,
    GetDistanceRequest locations, [FromQuery] Unit? unit = null)
{
    return await GetDistanceAsync(distanceProviderService, validator, context, locations, CalculationType.Spherical, unit);
}

async Task<IResult> GetFlatDistanceAsync(IDistanceProviderService distanceProviderService,
    IValidator<GetDistanceRequest> validator, HttpContext context,
    GetDistanceRequest locations, [FromQuery] Unit? unit = null)
{
    return await GetDistanceAsync(distanceProviderService, validator, context, locations, CalculationType.Flat, unit);
}

async Task<IResult> GetEllipsoidalDistanceAsync(IDistanceProviderService distanceProviderService,
    IValidator<GetDistanceRequest> validator, HttpContext context,
    GetDistanceRequest locations, [FromQuery] Unit? unit = null)
{
    return await GetDistanceAsync(distanceProviderService, validator, context, locations, CalculationType.Ellipsoidal, unit);
}

async Task<IResult> GetDistanceAsync(IDistanceProviderService distanceProviderService,
    IValidator<GetDistanceRequest> validator, HttpContext context,
    GetDistanceRequest locations, CalculationType type, [FromQuery] Unit? unit = null)
{
    var validationResult = await validator.ValidateAsync(locations);
    if (validationResult.IsValid is false)
    {
        return TypedResults.ValidationProblem(validationResult.ToDictionary());
    }

    var ipAddress = context.GetIpAddress();
    if (ipAddress is LocalIpV4 or LocalIpV6)
    {
        // Set a default ip address to be used on localhost for testing purposes
        ipAddress = ROAIpAddress;
    }

    return TypedResults.Ok(
        await distanceProviderService.GetDistanceAsync(locations.PointA, locations.PointB, ipAddress!, type, unit));
}

app.Run();
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

const string LocalIpV4 = "0.0.0.1";
const string LocalIpV6 = "::1";
const string USAIpAddress = "101.33.20.0";

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

app.MapPost("distance", GetDistance);

async Task<IResult> GetDistance(IDistanceProviderService distanceProviderService,
    IValidator<GetDistanceRequest> validator, HttpContext context,
    GetDistanceRequest locations, [FromQuery] Unit? unit = null)
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
        ipAddress = USAIpAddress;
    }

    return TypedResults.Ok(
        await distanceProviderService.GetDistanceAsync(locations.PointA, locations.PointB, ipAddress, unit));
}

app.Run();
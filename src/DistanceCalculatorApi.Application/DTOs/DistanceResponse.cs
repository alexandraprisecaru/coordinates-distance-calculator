namespace DistanceCalculatorApi.Application.DTOs;

using Enums;

public record DistanceResponse(double Distance, Unit? Unit);
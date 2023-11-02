namespace DistanceCalculatorApi.Application.DTOs;

public record GetDistanceRequest(CoordinateDto PointA, CoordinateDto PointB);
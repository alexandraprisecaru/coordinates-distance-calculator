namespace DistanceCalculatorAPI.Shared.Services;

using DistanceCalculatorApi.Application.DTOs;
using DistanceCalculatorApi.Application.Enums;
using DistanceCalculatorApi.Application.Interfaces;

public class DistanceCalculatorService : IDistanceCalculatorService
{
    public double CalculateDistance(CoordinateDto first, CoordinateDto second, Unit unit)
    {
        const double earthRadius = 6371;
        
        double a = 90 - second.Latitude;
        double b = 90 - first.Latitude;
        double phi = first.Longitude - second.Longitude;

        double cosP = Math.Cos(a) * Math.Cos(b) + Math.Sin(a) * Math.Sin(b) * Math.Cos(phi);
        double distance = earthRadius * Math.Acos(cosP);

        return unit is Unit.Metric
            ? distance
            : distance * 0.621371;
    }
}
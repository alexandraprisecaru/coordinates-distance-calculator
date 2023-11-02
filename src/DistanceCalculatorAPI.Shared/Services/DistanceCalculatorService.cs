namespace DistanceCalculatorAPI.Shared.Services;

using DistanceCalculatorApi.Application.DTOs;
using DistanceCalculatorApi.Application.Interfaces;
using GeoCoordinatePortable;

public class DistanceCalculatorService : IDistanceCalculatorService
{
    public double CalculateSphericalDistance(CoordinateDto pointA, CoordinateDto pointB)
    {
        const double earthRadius = 6371;

        double a = DegreesToRadians(90 - pointB.Latitude);
        double b = DegreesToRadians(90 - pointA.Latitude);
        double phi = DegreesToRadians(pointA.Longitude - pointB.Longitude);

        double cosP = Math.Cos(a) * Math.Cos(b) + Math.Sin(a) * Math.Sin(b) * Math.Cos(phi);

        return earthRadius * Math.Acos(cosP);
    }
    
    public double CalculateFlatDistance(CoordinateDto pointA, CoordinateDto pointB)
    {
        // Approximate distance in kilometers per degree of latitude
        const double kmPerDegreeLatitude = 111.32;

        double latitudeDistance = Math.Abs(pointA.Latitude - pointB.Latitude);
        double longitudeDistance = Math.Abs(pointA.Longitude - pointB.Longitude);

        double distance =
            Math.Sqrt(Math.Pow(
                          latitudeDistance * (kmPerDegreeLatitude * Math.Cos((pointA.Latitude + pointB.Latitude) / 2)), 2) +
                      Math.Pow(longitudeDistance * kmPerDegreeLatitude, 2));

        return distance;
    }
    
    public double CalculateEllipsoidalDistance(CoordinateDto pointA, CoordinateDto pointB)
    {
        var coord1 = new GeoCoordinate(pointA.Latitude, pointA.Longitude);
        var coord2 = new GeoCoordinate(pointB.Latitude, pointB.Longitude);

        double distance = coord1.GetDistanceTo(coord2) / 1000.0;
        return distance;
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
}
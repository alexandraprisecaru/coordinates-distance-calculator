namespace DistanceCalculatorApi.Application.Interfaces;

using DTOs;

public interface IDistanceCalculatorService
{
    /// <summary>
    /// Calculates the distance between two point considering the Earth spherical
    /// </summary>
    double CalculateSphericalDistance(CoordinateDto first, CoordinateDto second);
    
    /// <summary>
    /// Calculates the distance using Pythagorean formula, considering the Earth flat
    /// </summary>
    double CalculateFlatDistance(CoordinateDto pointA, CoordinateDto pointB);
    
    /// <summary>
    /// Calculates the distance considering the Earth ellipsoidal
    /// Uses GeoCoordinatePortable for calculation
    /// </summary>
    double CalculateEllipsoidalDistance(CoordinateDto pointA, CoordinateDto pointB);
}
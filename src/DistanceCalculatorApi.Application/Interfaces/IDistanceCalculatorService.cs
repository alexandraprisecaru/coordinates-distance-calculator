namespace DistanceCalculatorApi.Application.Interfaces;

using DTOs;
using Enums;

public interface IDistanceCalculatorService
{
    double CalculateDistance(CoordinateDto first, CoordinateDto second, Unit unit);
}
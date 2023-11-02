namespace DistanceCalculatorApi.Application.Validators;

using DTOs;
using FluentValidation;

public class LocationsValidator : AbstractValidator<GetDistanceRequest>
{
    public LocationsValidator()
    {
        var coordinateValidator = new CoordinateValidator();
        
        RuleFor(x => x.PointA).NotNull().SetValidator(coordinateValidator);
        RuleFor(x => x.PointB).NotNull().SetValidator(coordinateValidator);
    }
}
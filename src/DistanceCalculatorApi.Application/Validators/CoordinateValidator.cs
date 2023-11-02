namespace DistanceCalculatorApi.Application.Validators;

using DTOs;
using FluentValidation;

public class CoordinateValidator : AbstractValidator<CoordinateDto>
{
    public CoordinateValidator()
    {
        RuleFor(x => x.Latitude)
            .LessThanOrEqualTo(90)
            .GreaterThanOrEqualTo(-90);

        RuleFor(x => x.Longitude)
            .LessThanOrEqualTo(180)
            .GreaterThanOrEqualTo(-180);
    }
}

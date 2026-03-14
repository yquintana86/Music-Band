using FluentValidation;

namespace Application.RangePlusses.Command.CreateRangePlus;

public sealed class CreateRangePlusCommandValidator : AbstractValidator<CreateRangePlusCommand>
{
    public CreateRangePlusCommandValidator()
    {
        RuleFor(rv => rv.MinExperience).GreaterThanOrEqualTo(0);
        RuleFor(rv => rv.MaxExperience).GreaterThan(0);
        RuleFor(rv => rv.plus).GreaterThan(0);
        RuleFor(rv => rv.MinExperience).LessThan(rv => rv.MaxExperience);

    }
}


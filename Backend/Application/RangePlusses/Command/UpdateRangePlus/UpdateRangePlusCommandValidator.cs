using FluentValidation;

namespace Application.RangePlusses.Command.UpdateRangePlus;

public sealed class UpdateRangePlusCommandValidator : AbstractValidator<UpdateRangePlusCommand>
{
    public UpdateRangePlusCommandValidator()
    {
        RuleFor(rp => rp.Id).GreaterThan(0);
        RuleFor(rp => rp.MinExperience).GreaterThan(0);
        RuleFor(rp => rp.MaxExperience).GreaterThan(0);
        RuleFor(rp => rp.plus).GreaterThanOrEqualTo(0);
        RuleFor(rp => rp.MinExperience).LessThan(rp => rp.MaxExperience);
    }
}




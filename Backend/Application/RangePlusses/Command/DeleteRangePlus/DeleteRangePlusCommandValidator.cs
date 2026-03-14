using FluentValidation;

namespace Application.RangePlusses.Command.DeleteRangePlus;

public sealed class DeleteRangePlusCommandValidator : AbstractValidator<DeleteRangePlusCommand>
{
    public DeleteRangePlusCommandValidator()
    {
       RuleFor(rp => rp.Id).GreaterThan(0);
    }
}


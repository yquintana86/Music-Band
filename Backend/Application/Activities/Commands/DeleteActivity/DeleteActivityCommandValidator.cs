using FluentValidation;

namespace Application.Activities.Commands.DeleteActivity;

public sealed class DeleteActivityCommandValidator : AbstractValidator<DeleteActivityCommand>
{
    public DeleteActivityCommandValidator()
    {
        RuleFor(a => a.id).GreaterThan(0);
    }
}


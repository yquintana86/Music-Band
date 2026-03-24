using FluentValidation;

namespace Application.Activities.Commands.DeleteManyActivities;

public sealed class DeleteManyActivitiesCommandValidator : AbstractValidator<DeleteManyActivitiesCommand> 
{
    public DeleteManyActivitiesCommandValidator()
    {
        RuleFor(a => a).NotNull();
        RuleFor(a => a.ids).NotEmpty();
    }
} 

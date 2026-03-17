using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Activities.Commands.CreateAtivity;

public class CreateActivityCommandValidator : AbstractValidator<CreateActivityCommand>
{
    public CreateActivityCommandValidator()
    {
        RuleFor(a => a.Name).NotEmpty();
        RuleFor(a => a.Client).NotEmpty();
        RuleFor(a => a).Custom((a, vc) =>
        {
            if (a.Begin.HasValue && a.Begin.Value.Year < 2020)
                vc.AddFailure("Begin Date Shoud be After 2020");

            if (a.End.HasValue && a.End.Value.Year < 2020)
                vc.AddFailure("End Date Shoud be After 2020");

            if (a.Begin.HasValue && a.End.HasValue && a.Begin.Value > a.End.Value)
                vc.AddFailure("End Date Shoud be After begin Date");
        });
    }
}

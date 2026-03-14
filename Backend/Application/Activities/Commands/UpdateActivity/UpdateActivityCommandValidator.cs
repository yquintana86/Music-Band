using FluentValidation;

namespace Application.Activities.Commands.UpdateActivity;

public class UpdateActivityCommandValidator : AbstractValidator<UpdateActivityCommand>
{
    public UpdateActivityCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(a => a.Name).NotEmpty();
        RuleFor(a => a.Client).NotEmpty();
        RuleFor(a => a).Custom((a, vc) =>
        {
            if (a.Begin.HasValue && a.Begin.Value.Year < 2020)
                vc.AddFailure("Begin Date Shoud be After 2020");

            if (a.End.HasValue && a.End.Value.Year < 2020)
                vc.AddFailure("End Date Shoud be After 2020");

            if (a.Begin.HasValue && a.End.HasValue && a.Begin.Value < a.End.Value)
                vc.AddFailure("End Date Shoud be After begin Date");
        });



    }
}

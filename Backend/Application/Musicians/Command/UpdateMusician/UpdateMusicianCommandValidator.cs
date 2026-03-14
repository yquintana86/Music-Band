using FluentValidation;

namespace Application.Musicians.Command.UpdateMusician;

public sealed class UpdateMusicianCommandValidator : AbstractValidator<UpdateMusicianCommand>
{
    public UpdateMusicianCommandValidator()
    {
        RuleFor(m => m.Id).GreaterThan(0);
        RuleFor(m => m.FirstName).NotEmpty();
        RuleFor(m => m.LastName).NotEmpty();
        RuleFor(m => m.Experience).GreaterThan(0).LessThanOrEqualTo(100);
        RuleFor(m => m.Age).GreaterThan(0).LessThanOrEqualTo(150);
    }
}



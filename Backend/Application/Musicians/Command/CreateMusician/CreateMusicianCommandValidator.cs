using FluentValidation;

namespace Application.Musicians.Command.CreateMusician;

public class CreateMusicianCommandValidator : AbstractValidator<CreateMusicianCommand>
{
    public CreateMusicianCommandValidator()
    {
        RuleFor(m => m.FirstName).NotEmpty();
        RuleFor(m => m.LastName).NotEmpty();
        RuleFor(m => m.Experience).GreaterThan(0).LessThanOrEqualTo(100);
        RuleFor(m => m.Age).GreaterThan(0).LessThanOrEqualTo(150);
    }
}


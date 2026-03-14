using FluentValidation;

namespace Application.Musicians.Command.DeleteMusician;

public sealed class DeleteMusicianCommandValidator : AbstractValidator<DeleteMusicianCommand>
{
    public DeleteMusicianCommandValidator()
    {
            RuleFor(m => m.Id).GreaterThan(0);
    }
}

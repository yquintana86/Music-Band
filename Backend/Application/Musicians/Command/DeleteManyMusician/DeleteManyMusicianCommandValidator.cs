using FluentValidation;

namespace Application.Musicians.Command.DeleteManyMusician;

public sealed class DeleteManyMusicianCommandValidator : AbstractValidator<DeleteManyMusicianCommand>
{
    public DeleteManyMusicianCommandValidator()
    {
        RuleFor(m => m).NotNull();
        RuleFor(m => m.ids).NotEmpty();
    }
}
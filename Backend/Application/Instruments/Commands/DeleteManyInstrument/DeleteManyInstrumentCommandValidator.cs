using FluentValidation;

namespace Application.Instruments.Commands.DeleteManyInstrument;

public class DeleteManyInstrumentCommandValidator : AbstractValidator<DeleteManyInstrumentCommand>
{
    public DeleteManyInstrumentCommandValidator()
    {
        RuleFor(ic => ic).NotNull();
        RuleFor(ic => ic.instrumentIds).NotEmpty();
    }
}

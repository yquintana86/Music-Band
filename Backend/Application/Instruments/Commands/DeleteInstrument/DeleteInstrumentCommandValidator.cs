using FluentValidation;

namespace Application.Instrument.Commands.DeleteInstrument;

public sealed class DeleteInstrumentCommandValidator : AbstractValidator<DeleteInstrumentCommand>
{
    public DeleteInstrumentCommandValidator() 
    {
        RuleFor(i => i.Id).GreaterThan(0);
    }
}

using FluentValidation;

namespace Application.Instrument.Commands.UpdateInstrument;

public sealed class UpdateInstrumentCommandValidator : AbstractValidator<UpdateInstrumentCommand>
{
    public UpdateInstrumentCommandValidator()
    {
        RuleFor(i => i.Id).GreaterThan(0);
        RuleFor(i => i.MusicianId).GreaterThan(0);
        RuleFor(i => i.Name).NotEmpty();
    }
}
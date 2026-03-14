using FluentValidation;

namespace Application.Instrument.Commands.CreateInstrument;

public sealed class CreateInstrumentCommandvalidator : AbstractValidator<CreateInstrumentCommand>
{
    public CreateInstrumentCommandvalidator()
    {
        RuleFor(i => i.MusicianId).GreaterThan(0);
        RuleFor(i => i.Name).NotEmpty();
    }
}


using FluentValidation;

namespace Application.Instrument.Queries.SearchInstrumentbyId;

public class GetMusicalInstrumentByIdQueryValidator : AbstractValidator<GetMusicalInstrumentByIdQuery>
{
    public GetMusicalInstrumentByIdQueryValidator()
    {
        RuleFor(si => si.id).GreaterThan(0);
    }
}

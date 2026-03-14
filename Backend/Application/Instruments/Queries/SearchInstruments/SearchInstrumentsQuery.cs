using Application.Abstractions.Messaging;
using Shared.Models.Instrument;

namespace Application.Instrument.Queries.GetInstruments;

public sealed record SearchInstrumentsQuery : IQuery<IEnumerable<MusicalInstrumentResponse>>;

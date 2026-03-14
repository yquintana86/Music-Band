using Application.Abstractions.Messaging;
using Shared.Models.Instrument;

namespace Application.Instrument.Queries.SearchInstrumentbyId;

public sealed record GetMusicalInstrumentByIdQuery(int id) : IQuery<MusicalInstrumentResponse>;

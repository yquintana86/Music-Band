using Application.Abstractions.Messaging;

namespace Application.Instruments.Queries.GetMostUsedInstruments;


public sealed record GetMostUsedInstrumentQuery(int InstrumentQtyToSearch) : IQuery<MostUsedInstrumentResponse>;
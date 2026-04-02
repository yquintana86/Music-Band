using Application.Abstractions.Messaging;
using Shared.Models.Musician;

namespace Application.Instruments.GetMusiciansByInstrument;

public sealed record MusiciansByInstrumentQuery(int instrumentId): IQuery<IEnumerable<MusicianResponse>>;
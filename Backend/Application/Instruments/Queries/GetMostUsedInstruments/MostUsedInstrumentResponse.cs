using Domain.Entities;
using System.Xml.Linq;

namespace Application.Instruments.Queries.GetMostUsedInstruments;

public sealed record MostUsedInstrumentResponse(Dictionary<string, IEnumerable<Musician>> MusiciansByInstrumentName);

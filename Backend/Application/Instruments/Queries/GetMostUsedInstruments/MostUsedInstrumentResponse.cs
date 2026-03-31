using Domain.Entities;
using Shared.Models.Musician;
using SharedLib.Models.Common;
using System.Xml.Linq;

namespace Application.Instruments.Queries.GetMostUsedInstruments;

public sealed record MostUsedInstrumentResponse(Dictionary<string, IEnumerable<MusicianResponse>> MusiciansByInstrumentName);

using Application.Abstractions.Messaging;
using Shared.Common;

namespace Application.Musicians.Query.GetMusicianAverageByPlayedInstrumentsType;

public sealed record GetMusicianAverageByInstrumentsQuery(IEnumerable<int> instrumentIds) : IQuery<decimal>;
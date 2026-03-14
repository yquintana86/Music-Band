using Application.Abstractions.Messaging;

namespace Application.Musicians.Query.IternationalActivitiesByMusician;

public sealed record InternationalActivitiesByMusicianQuery(int Id) : IQuery<int>;
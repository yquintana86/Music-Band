using Application.Abstractions.Messaging;
using Shared.Models.Musician;

namespace Application.Musicians.Query.SearchMusicianById;

public sealed record GetMusicianByIdQuery(int Id) : IQuery<MusicianResponse?>;

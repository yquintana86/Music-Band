using Application.Abstractions.Messaging;
using Shared.Models.Musician;
using System.Collections.Generic;

namespace Application.Musicians.Query.GetMusicians;

public sealed record SearchMusiciansQuery : IQuery<IEnumerable<MusicianResponse>>;

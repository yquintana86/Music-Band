using Application.Abstractions.Messaging;
using Application.Musicians.Query.MusicianDashboardGenerics.Dtos;

namespace Application.Musicians.Query.MusicianDashboardGenerics;

public sealed record MusicianDashboardGenericsQuery(DateTime? startDate, DateTime? endDate) : IQuery<MusicianDashboardGenericsResponse>;
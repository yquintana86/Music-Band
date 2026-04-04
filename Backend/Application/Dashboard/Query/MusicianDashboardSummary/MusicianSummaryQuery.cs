using Application.Abstractions.Messaging;
using Application.Dashboard.Query.MusicianDashboardGenerics.Dtos;

namespace Application.Dashboard.Query.MusicianDashboardGenerics;

public sealed record MusicianSummaryQuery(DateTime? startDate, DateTime? endDate) : IQuery<MusicianSummaryResponse>;
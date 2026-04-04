namespace Application.Dashboard.Query.MusicianDashboardGenerics.Dtos;

public sealed record MusicianSummaryResponse(int musicianQty, int internationalQty, double salaryAvg, decimal ageAvg);


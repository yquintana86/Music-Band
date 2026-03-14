using Application.Abstractions.Messaging;
using Shared.Models.Musician;
using SharedLib.Models.Common;

namespace Application.Musicians.Query.SearchMusiciansByFilter;

public sealed class SearchMusicianByFilterQuery : PagingFilter, IQuery<PagedResult<MusicianResponse>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? FromAge { get; set; }
    public int? ToAge { get; set; }
    public int? FromExperience { get; set; }
    public int? ToExperience { get; set; }
    public double? FromBasicSalary { get; set; }
    public double? ToBasicSalary { get; set; }
    public int? InstrumentId { get; set; }
}
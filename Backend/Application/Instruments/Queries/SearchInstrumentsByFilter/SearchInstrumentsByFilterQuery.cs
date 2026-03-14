using Application.Abstractions.Messaging;
using Shared.Common;
using Shared.Models.Instrument;
using SharedLib.Models.Common;

namespace Application.Instruments.Queries.SearchInstrumentsByFilter;

public sealed class SearchInstrumentsByFilterQuery : PagingFilter, IQuery<PagedResult<MusicalInstrumentResponse>>
{
    public string? Name { get; set; }
    public string? Country { get; set; }
    public InstrumentType? Type { get; set; }
}
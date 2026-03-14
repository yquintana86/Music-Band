using Application.Abstractions.Messaging;
using Shared.Models.RangePlus;
using SharedLib.Models.Common;
using System.Linq.Expressions;

namespace Application.RangePlusses.Query.SearchRangePlusByFilter;

public sealed class SearchRangePlusByFilterQuery : PagingFilter, IQuery<PagedResult<RangePlusResponse>>
{
    public int? Experience { get; set; }
    public decimal? FromPlus { get; set; }
    public decimal? ToPlus { get; set; }
}
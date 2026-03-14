using Application.Abstractions.Messaging;
using Shared.Models.Activity;
using SharedLib.Models.Common;

namespace Application.Activities.Queries.SearchActivitiesByFilter;

public sealed class SearchActivitiesByFilterQuery : PagingFilter, IQuery<PagedResult<ActivityResponse>>
{
    public bool? International { get; set; }
    public DateTime? Begin { get; set; }
    public DateTime? End { get; set; }
}
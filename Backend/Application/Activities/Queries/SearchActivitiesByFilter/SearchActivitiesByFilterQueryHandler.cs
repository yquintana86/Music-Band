using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Shared.Models.Activity;
using SharedLib.Models.Common;

namespace Application.Activities.Queries.SearchActivitiesByFilter;

internal sealed class SearchActivitiesByFilterQueryHandler : IQueryHandler<SearchActivitiesByFilterQuery, PagedResult<ActivityResponse>>
{
    private readonly IActivityRepository _activityRepository;
    private readonly ILogger<SearchActivitiesByFilterQueryHandler> _logger;

    public SearchActivitiesByFilterQueryHandler(IActivityRepository activityRepository, ILogger<SearchActivitiesByFilterQueryHandler> logger)
    {
        _activityRepository = activityRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<PagedResult<ActivityResponse>>> Handle(SearchActivitiesByFilterQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var paged = await _activityRepository.SearchByFilterAsync(request, cancellationToken);

            var pagedResponse = new PagedResult<ActivityResponse>
            {
                Currentpage = paged.Currentpage,
                HasNextPage = paged.HasNextPage,
                ItemCount = paged.ItemCount,
                PageCount = paged.PageCount,
                PageSize = paged.PageSize,
                TotalItemCount = paged.TotalItemCount,
                Results = paged.Results?.Select(a => new ActivityResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Begin = a.Begin,
                    End = a.End,
                    Client = a.Client,
                    International = a.International,
                    Description = a.Description,
                    Price = a.Price,
                }).ToList()
            };

            return ApiOperationResult.Success(pagedResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<PagedResult<ActivityResponse>>(ApiOperationError.Failure(ex.GetType().ToString(), ex.Message));
        }

    }
}


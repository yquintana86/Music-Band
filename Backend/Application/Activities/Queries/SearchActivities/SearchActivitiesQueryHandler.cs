using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Models.Activity;
using SharedLib.Models.Common;

namespace Application.Activities.Queries.ListActivities;

internal sealed class SearchActivitiesQueryHandler : IQueryHandler<SearchActivitiesQuery, IEnumerable<ActivityResponse>>
{
    private readonly IActivityRepository _activityRepository;
    private readonly ILogger<SearchActivitiesQueryHandler> _logger;

    public SearchActivitiesQueryHandler(IActivityRepository activityRepository, ILogger<SearchActivitiesQueryHandler> logger)
    {
        _activityRepository = activityRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<IEnumerable<ActivityResponse>>> Handle(SearchActivitiesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var list = await _activityRepository.GetAllAsync(cancellationToken);

            var result = new List<ActivityResponse>();

            if (list is not null && list.Any())
            {
                result = list.Select(a => new ActivityResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Client = a.Client,
                    Description = a.Description,
                    International = a.International,
                    Begin = a.Begin,
                    End = a.End
                }).ToList();
            }

            return ApiOperationResult.Success<IEnumerable<ActivityResponse>>(result);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<IEnumerable<ActivityResponse>>(ex.GetType().Name,ex.Message,ApiErrorType.Failure);

        }
    }
}

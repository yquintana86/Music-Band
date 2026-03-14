using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Shared.Models.Activity;
using SharedLib.Models.Common;

namespace Application.Activities.Queries.SearchActivityById;

internal sealed class SearchActivityByIdQueryHandler : IQueryHandler<SearchActivityByIdQuery, ActivityResponse>
{

    private readonly IActivityRepository _activityRepository;
    private readonly ILogger<SearchActivityByIdQueryHandler> _logger;

    public SearchActivityByIdQueryHandler(IActivityRepository activityRepository, ILogger<SearchActivityByIdQueryHandler> logger)
    {
        _activityRepository = activityRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<ActivityResponse>> Handle(SearchActivityByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            int actId = request.Id;

            Activity? activity = await _activityRepository.GetByIdAsync(actId, cancellationToken);
            if (activity is null)
                return ApiOperationResult.Fail<ActivityResponse>(ActivityError.ExistActivityNotFoundId(actId));

            var model = new ActivityResponse
            {
                Id = activity.Id,
                Name = activity.Name,
                Client = activity.Client,
                Description = activity.Description,
                International = activity.International,
                Begin = activity.Begin,
                End = activity.End
            };

            return ApiOperationResult.Success(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<ActivityResponse>(ex.GetType().Name, ex.Message, ApiErrorType.Failure);
        }
    }
}

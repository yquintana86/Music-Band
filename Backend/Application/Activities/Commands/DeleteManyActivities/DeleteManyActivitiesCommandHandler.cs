using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Activities.Commands.DeleteManyActivities;

internal sealed class DeleteManyActivitiesCommandHandler : ICommandHandler<DeleteManyActivitiesCommand>
{

    private readonly IActivityRepository _activityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteManyActivitiesCommandHandler> _logger;

    public DeleteManyActivitiesCommandHandler(IActivityRepository activityRepository, IUnitOfWork unitOfWork, ILogger<DeleteManyActivitiesCommandHandler> logger)
    {
        _activityRepository = activityRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApiOperationResult> Handle(DeleteManyActivitiesCommand request, CancellationToken cancellationToken)
    {

        try
        {
            await _activityRepository.DeleteManyAsync(request.ids);
            await _unitOfWork.SaveChangesAsync();

            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return ApiOperationResult.Fail(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}

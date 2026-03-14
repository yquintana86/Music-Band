using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Activities.Commands.DeleteActivity;

public sealed class DeleteActivityCommandHandler : ICommandHandler<DeleteActivityCommand>
{

    private readonly IActivityRepository _activityRepository;
    private readonly ILogger<DeleteActivityCommandHandler> _logger;

    public DeleteActivityCommandHandler(IActivityRepository activityRepository, ILogger<DeleteActivityCommandHandler> logger)
    {
        _activityRepository = activityRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var actId = request.id;
            var exist = await _activityRepository.ExistIdAsync(actId, cancellationToken);
            if (!exist)
                return ApiOperationResult.Fail(ActivityError.DeleteActivityNotFoundId(actId));

            await _activityRepository.DeleteAsync(actId);
            return ApiOperationResult.Success();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail(ex.GetType().Name, ex.Message, ApiErrorType.Failure);
        }
    }
}


using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Activities.Commands.UpdateActivity;

internal class UpdateActivityCommandHandler : ICommandHandler<UpdateActivityCommand>
{
    private readonly IActivityRepository _activityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateActivityCommandHandler> _logger;

    public UpdateActivityCommandHandler(IActivityRepository activityRepository, ILogger<UpdateActivityCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _activityRepository = activityRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiOperationResult> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var activityDb = await _activityRepository.GetByIdAsync(request.Id,cancellationToken);
            if (activityDb is null)
                return ApiOperationResult.Fail(ActivityError.UpdateActivityNotFoundId(request.Id));

            activityDb.Name = request.Name;
            activityDb.Description = request.Description;
            activityDb.Client = request.Client;
            activityDb.International = request.International;
            activityDb.Begin = request.Begin;
            activityDb.End = request.End;
            
            

            await _unitOfWork.SaveChangesAsync();
            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail(ex.GetType().Name, ex.Message, ApiErrorType.Failure);
        }
    }
}

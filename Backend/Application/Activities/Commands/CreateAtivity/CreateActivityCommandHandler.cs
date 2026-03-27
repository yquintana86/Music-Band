using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Activities.Commands.CreateAtivity;

internal class UpdateActivityCommandHandler : ICommandHandler<CreateActivityCommand>
{
    private readonly IActivityRepository _activityRepository;
    private readonly IMusicianRepository _musicianRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateActivityCommandHandler> _logger;

    public UpdateActivityCommandHandler(IActivityRepository activityRepository, ILogger<UpdateActivityCommandHandler> logger, IUnitOfWork unitOfWork, IMusicianRepository musicianRepository)
    {
        _activityRepository = activityRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _musicianRepository = musicianRepository;
    }

    public async Task<ApiOperationResult> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var musicians = await _musicianRepository.GetAllAsync(m => request.MusiciansId.Contains(m.Id), cancellationToken);

            var activity = new Activity
            {
                Name = request.Name,
                Client = request.Client,
                Description = request.Description,
                International = request.International,
                Begin = request.Begin,
                End = request.End,
                //TODO: Update Musician Activity
            };

            _activityRepository.Add(activity);
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

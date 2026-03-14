using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Musicians.Query.IternationalActivitiesByMusician;

internal sealed class IternationalActivitiesByMusicianQueryHandler : IQueryHandler<InternationalActivitiesByMusicianQuery, int>
{
    private readonly IActivityRepository _activityRepository;
    private readonly IMusicianRepository _musicianRepository;
    private readonly ILogger<IternationalActivitiesByMusicianQueryHandler> _logger;

    public IternationalActivitiesByMusicianQueryHandler(IActivityRepository activityRepository, IMusicianRepository musicianRepository, ILogger<IternationalActivitiesByMusicianQueryHandler> logger)
    {
        _activityRepository = activityRepository;
        _musicianRepository = musicianRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<int>> Handle(InternationalActivitiesByMusicianQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.Id == 0)
            {
                return ApiOperationResult.Fail<int>(MusicianError.InvalidId(request?.Id ?? 0));
            }

            var musicianDb = await _musicianRepository.GetByIdAsync(request.Id);
            if (musicianDb is null)
            {
                return ApiOperationResult.Fail<int>(MusicianError.NotFound(request?.Id ?? 0));
            }

            var internationalActivities = await _activityRepository.GetInternationalActivitiesByMusicianIdAsync(request.Id, cancellationToken);

            return ApiOperationResult.Success(internationalActivities?.Count() ?? 0);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<int>(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}
 
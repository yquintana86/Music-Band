using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Musicians.Query.MusicianDashboardGenerics.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Musicians.Query.MusicianDashboardGenerics;

internal sealed class MusicianDashboardGenericsQueryHandler : IQueryHandler<MusicianDashboardGenericsQuery, MusicianDashboardGenericsResponse>
{
    private readonly IMusicianRepository _musicianRepository;
    private readonly ILogger<MusicianDashboardGenericsQueryHandler> _logger;

    public MusicianDashboardGenericsQueryHandler(IMusicianRepository musicianRepository, ILogger<MusicianDashboardGenericsQueryHandler> logger)
    {
        _musicianRepository = musicianRepository;
        _logger = logger;
    }

    async Task<ApiOperationResult<MusicianDashboardGenericsResponse>> IRequestHandler<MusicianDashboardGenericsQuery, ApiOperationResult<MusicianDashboardGenericsResponse>>.Handle(MusicianDashboardGenericsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _musicianRepository.GetMusicianDashboardGenericsAsync(request.startDate, request.endDate, cancellationToken);
            return ApiOperationResult.Success(new MusicianDashboardGenericsResponse
                (
                response.musicianQty,
                response.internationalQty,
                response.salaryAvg,
                response.ageAvg
                ));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return ApiOperationResult.Fail<MusicianDashboardGenericsResponse>(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}


using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Dashboard.Query.MusicianDashboardGenerics.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Dashboard.Query.MusicianDashboardGenerics;

internal sealed class MusicianSummaryQueryHandler : IQueryHandler<MusicianSummaryQuery, MusicianSummaryResponse>
{
    private readonly IMusicianRepository _musicianRepository;
    private readonly ILogger<MusicianSummaryQueryHandler> _logger;

    public MusicianSummaryQueryHandler(IMusicianRepository musicianRepository, ILogger<MusicianSummaryQueryHandler> logger)
    {
        _musicianRepository = musicianRepository;
        _logger = logger;
    }

    async Task<ApiOperationResult<MusicianSummaryResponse>> IRequestHandler<MusicianSummaryQuery, ApiOperationResult<MusicianSummaryResponse>>.Handle(MusicianSummaryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _musicianRepository.GetMusicianDashboardGenericsAsync(request.startDate, request.endDate, cancellationToken);
            return ApiOperationResult.Success(new MusicianSummaryResponse
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
            return ApiOperationResult.Fail<MusicianSummaryResponse>(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}


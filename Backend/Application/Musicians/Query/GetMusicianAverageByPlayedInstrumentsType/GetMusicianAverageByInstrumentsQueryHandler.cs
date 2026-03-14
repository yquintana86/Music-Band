using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Shared.Common;
using SharedLib.Models.Common;

namespace Application.Musicians.Query.GetMusicianAverageByPlayedInstrumentsType;

internal sealed class GetMusicianAverageByInstrumentsQueryHandler : IQueryHandler<GetMusicianAverageByInstrumentsQuery, decimal>
{
    private readonly IMusicianRepository _musicianRepository;
    private readonly IInstrumentRepository _instrumentRepository;
    private readonly ILogger<GetMusicianAverageByInstrumentsQueryHandler> _logger;

    public GetMusicianAverageByInstrumentsQueryHandler(IMusicianRepository musicianRepository, ILogger<GetMusicianAverageByInstrumentsQueryHandler> logger, IInstrumentRepository instrumentRepository)
    {
        _musicianRepository = musicianRepository;
        _logger = logger;
        _instrumentRepository = instrumentRepository;
    }

    public async Task<ApiOperationResult<decimal>> Handle(GetMusicianAverageByInstrumentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.instrumentIds is null || !request.instrumentIds.Any())
            {
                return ApiOperationResult.Fail<decimal>(MusicianError.InvalidId(0));
            }

            if (Enum.TryParse<InstrumentType>(request.InstrumentType.ToString(), out var instrumentType))
            {
                return ApiOperationResult.Fail<decimal>(InstrumentError.InvalidType());
            }
            var instrumentsDb = await _instrumentRepository.GetAllAsync(i => request.instrumentIds.Contains(i.Id), cancellationToken);

            if (instrumentsDb is null || instrumentsDb.Count() != request.instrumentIds.Count() || instrumentsDb.Any(i => i.Type != request.InstrumentType))
            {
                return ApiOperationResult.Fail<decimal>(InstrumentError.NotFound());
            }

            var average = await _musicianRepository.GetMusicianAverageByPlayedInstrumentsTypeAsync(request.instrumentIds, cancellationToken);
            return ApiOperationResult.Success<decimal>(average);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<decimal>(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}

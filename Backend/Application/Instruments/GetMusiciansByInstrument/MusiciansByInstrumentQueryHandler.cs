using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Shared.Models.Musician;
using SharedLib.Models.Common;

namespace Application.Instruments.GetMusiciansByInstrument;

internal sealed class MusiciansByInstrumentQueryHandler : IQueryHandler<MusiciansByInstrumentQuery, IEnumerable<MusicianResponse>>
{
    private readonly IInstrumentRepository _instrumentRepository;
    private readonly ILogger<MusiciansByInstrumentQueryHandler> _logger;
    public MusiciansByInstrumentQueryHandler(IInstrumentRepository instrumentRepository, ILogger<MusiciansByInstrumentQueryHandler> logger)
    {
        _instrumentRepository = instrumentRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<IEnumerable<MusicianResponse>>> Handle(MusiciansByInstrumentQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.instrumentId == 0)
            {
                return ApiOperationResult.Fail<IEnumerable<MusicianResponse>>(InstrumentError.InvalidId());
            }

            var response = await _instrumentRepository.GetMusiciansByInstrumentIdAsync(request.instrumentId, cancellationToken);

            var result = response.Select(m => new MusicianResponse
            {
                Id = m.Id,
                Age = m.Age,
                BasicSalary = m.BasicSalary,
                Experience = m.Experience,
                FirstName = m.FirstName,
                LastName = m.LastName,
                MiddleName = m.MiddleName,
            }).ToList();

            return ApiOperationResult.Success((IEnumerable<MusicianResponse>)result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            
            return ApiOperationResult.Fail<IEnumerable<MusicianResponse>>(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}

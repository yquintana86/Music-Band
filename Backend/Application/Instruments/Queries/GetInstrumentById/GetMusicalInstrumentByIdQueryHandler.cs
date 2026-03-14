using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Shared.Models.Instrument;
using SharedLib.Models.Common;

namespace Application.Instrument.Queries.SearchInstrumentbyId;

internal sealed class GetMusicalInstrumentByIdQueryHandler : IQueryHandler<GetMusicalInstrumentByIdQuery, MusicalInstrumentResponse>
{
    private readonly IInstrumentRepository _instrumentRepository;
    private readonly ILogger<GetMusicalInstrumentByIdQueryHandler> _logger;

    public GetMusicalInstrumentByIdQueryHandler(IInstrumentRepository instrumentRepository, ILogger<GetMusicalInstrumentByIdQueryHandler> logger)
    {
        _instrumentRepository = instrumentRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<MusicalInstrumentResponse>> Handle(GetMusicalInstrumentByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.id <= 0)
                return ApiOperationResult.Fail<MusicalInstrumentResponse>(InstrumentError.InvalidId());

            var instrument = await _instrumentRepository.GetByIdAsync(request.id, cancellationToken);

            MusicalInstrumentResponse? modelResponse = null;

            if (instrument is not null)
            {
                modelResponse = new MusicalInstrumentResponse
                {
                    Id = instrument.Id,
                    Name = instrument.Name,
                    Country = instrument.Country,
                    Description = instrument.Description,
                    MusicianId = instrument.MusicianId,
                };
            }

            return ApiOperationResult.Success(modelResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has ocurred: {}", ex.Message);
            return ApiOperationResult.Fail<MusicalInstrumentResponse>(ApiOperationError.Failure(ex.GetType().Name, ex.Message));
        }
    }
}

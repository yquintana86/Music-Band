using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Models.Instrument;
using SharedLib.Models.Common;

namespace Application.Instrument.Queries.GetInstruments;

internal sealed class SearchInstrumentsQueryHandler : IQueryHandler<SearchInstrumentsQuery, IEnumerable<MusicalInstrumentResponse>>
{
    private readonly IInstrumentRepository _instrumentRepository;
    private readonly ILogger<SearchInstrumentsQueryHandler> _logger;
    public SearchInstrumentsQueryHandler(IInstrumentRepository instrumentRepository, ILogger<SearchInstrumentsQueryHandler> logger)
    {
        _instrumentRepository = instrumentRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<IEnumerable<MusicalInstrumentResponse>>> Handle(SearchInstrumentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var instruments = await _instrumentRepository.GetAllAsync(m => true, cancellationToken);

            IEnumerable<MusicalInstrumentResponse> result = new List<MusicalInstrumentResponse>();
            if (instruments is not null && instruments.Any())
            {
                result = instruments.Select(i => new MusicalInstrumentResponse
                {
                    Id = i.Id,
                    Name = i.Name,
                    Country = i.Country,
                    Description = i.Description,
                    Type = i.Type,
                    MusicianId = i.MusicianId
                });
            }

            return ApiOperationResult.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has ocurred: {}", ex.Message);
            return ApiOperationResult.Fail<IEnumerable<MusicalInstrumentResponse>>(ApiOperationError.Failure(ex.GetType().Name, ex.Message));
        }
    }
}

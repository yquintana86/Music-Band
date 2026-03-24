using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Models.Instrument;
using SharedLib.Models.Common;

namespace Application.Instruments.Queries.SearchInstrumentsByFilter;

internal sealed class SearchInstrumentsByFilterQueryHandler : IQueryHandler<SearchInstrumentsByFilterQuery, PagedResult<MusicalInstrumentResponse>>
{
    private readonly IInstrumentRepository _instrumentRepository;
    private readonly ILogger<SearchInstrumentsByFilterQueryHandler> _logger;

    public SearchInstrumentsByFilterQueryHandler(IInstrumentRepository instrumentRepository, ILogger<SearchInstrumentsByFilterQueryHandler> logger)
    {
        _instrumentRepository = instrumentRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<PagedResult<MusicalInstrumentResponse>>> Handle(SearchInstrumentsByFilterQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _instrumentRepository.SearchByFilterAsync(request, cancellationToken);

            var pagedResult = new PagedResult<MusicalInstrumentResponse>
            {
                Currentpage = result.Currentpage,
                HasNextPage = result.HasNextPage,
                ItemCount = result.ItemCount,
                PageCount = result.PageCount,
                PageSize = result.PageSize,
                TotalItemCount = result.TotalItemCount,
                Results = result.Results?.Select(i => new MusicalInstrumentResponse
                {
                    Id = i.Id,
                    Name = i.Name,
                    Type = i.Type,
                    Country = i.Country,
                    Description = i.Description,
                    MusicianId = i.MusicianId,
                    MusicianName = $"{i.Musician.FirstName} {i.Musician.LastName}",
                }).ToList(),
            };

            return ApiOperationResult.Success(pagedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<PagedResult<MusicalInstrumentResponse>>(ApiOperationError.Failure(ex.GetType().ToString(), ex.Message));

            
        }
    }
}

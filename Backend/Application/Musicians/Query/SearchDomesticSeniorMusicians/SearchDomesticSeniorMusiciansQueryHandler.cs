using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Musicians.Query.SearchDomesticSeniorMusicians;

internal sealed class SearchDomesticSeniorMusiciansQueryHandler : IQueryHandler<SearchDomesticSeniorMusiciansQuery, int>
{
    private readonly IMusicianRepository _musicianRepository;
    private readonly ILogger<SearchDomesticSeniorMusiciansQueryHandler> _logger;

    public SearchDomesticSeniorMusiciansQueryHandler(IMusicianRepository musicianRepository, ILogger<SearchDomesticSeniorMusiciansQueryHandler> logger)
    {
        _musicianRepository = musicianRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<int>> Handle(SearchDomesticSeniorMusiciansQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if ((request?.age ?? 0) <= 15)
            {
                return ApiOperationResult.Fail<int>(MusicianError.InvalidAgeFilter());
            }

            var musicians = await _musicianRepository.SearchNoInternationalMusicianOlderThanAge(request!.age, cancellationToken);
            return ApiOperationResult.Success<int>(musicians?.Count() ?? 0);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<int>(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}



using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Models.Musician;
using SharedLib.Models.Common;

namespace Application.Musicians.Query.GetMusicians;

internal sealed class SearchMusiciansQueryHandler : IQueryHandler<SearchMusiciansQuery, IEnumerable<MusicianResponse>>
{

    private readonly IMusicianRepository _musicianRepository;
    private readonly ILogger<SearchMusiciansQueryHandler> _logger;

    public SearchMusiciansQueryHandler(IMusicianRepository musicianRepository, ILogger<SearchMusiciansQueryHandler> logger)
    {
        _musicianRepository = musicianRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<IEnumerable<MusicianResponse>>> Handle(SearchMusiciansQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _musicianRepository.GetAllAsync(m => true, cancellationToken);
            
            IEnumerable<MusicianResponse> models = new List<MusicianResponse>();

            if (result is not null && result.Any())
            {
                models = result.Select(m => new MusicianResponse
                {
                    Id = m.Id,
                    FirstName = m.FirstName,
                    MiddleName = m.MiddleName,
                    LastName = m.LastName,
                    Age = m.Age,
                    Experience = m.Experience, 
                }).ToList();
            }

            return ApiOperationResult.Success(models);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<IEnumerable<MusicianResponse>>(ex.GetType().Name, ex.Message, ApiErrorType.Failure);
        }
    }
}

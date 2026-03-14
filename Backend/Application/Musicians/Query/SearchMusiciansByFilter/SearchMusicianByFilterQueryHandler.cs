using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Shared.Models.Musician;
using SharedLib.Models.Common;

namespace Application.Musicians.Query.SearchMusiciansByFilter;

internal sealed class SearchMusicianByFilterQueryHandler : IQueryHandler<SearchMusicianByFilterQuery, PagedResult<MusicianResponse>>
{
    private readonly IMusicianRepository _musicianRepository;
    private readonly ILogger<SearchMusicianByFilterQueryHandler> _logger;

    public SearchMusicianByFilterQueryHandler(IMusicianRepository musicianRepository, ILogger<SearchMusicianByFilterQueryHandler> logger)
    {
        _musicianRepository = musicianRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<PagedResult<MusicianResponse>>> Handle(SearchMusicianByFilterQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.FromAge.HasValue && request.ToAge.HasValue &&
            request.FromAge > request.ToAge)
            {
                return ApiOperationResult.Fail<PagedResult<MusicianResponse>>(MusicianError.InvalidAgeFilter());
            }

            if (request.FromExperience.HasValue && request.ToExperience.HasValue &&
                request.FromExperience > request.ToExperience)
            {
                return ApiOperationResult.Fail<PagedResult<MusicianResponse>>(MusicianError.InvalidExperienceFilter());
            }

            if (request.FromBasicSalary.HasValue && request.ToBasicSalary.HasValue &&
                request.FromBasicSalary > request.ToBasicSalary)
            {
                return ApiOperationResult.Fail<PagedResult<MusicianResponse>>(MusicianError.InvalidBasicSalaryFilter());
            }

            var paged = await _musicianRepository.SearchByFilterAsync(request, cancellationToken);

            var pagedResult = new PagedResult<MusicianResponse>
            {
                Currentpage = paged.Currentpage,
                HasNextPage = paged.HasNextPage,
                ItemCount = paged.ItemCount,
                PageCount = paged.PageCount,
                PageSize = paged.PageSize,
                TotalItemCount = paged.TotalItemCount,
                Results = paged.Results?.Select(m => new MusicianResponse
                {
                    Id = m.Id,
                    FirstName = m.FirstName,
                    MiddleName = m.MiddleName,
                    LastName = m.LastName,
                    Age = m.Age,
                    Experience = m.Experience,
                    BasicSalary = m.BasicSalary,
                }).ToList()
            };

            return ApiOperationResult.Success(pagedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<PagedResult<MusicianResponse>>(ApiOperationError.Failure(ex.GetType().ToString(), ex.Message));
            
        }

    }
}

using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Shared.Models.RangePlus;
using SharedLib.Models.Common;

namespace Application.RangePlusses.Query.SearchRangePlusByFilter;

internal sealed class SearchRangePlusByFilterQueryHandler : IQueryHandler<SearchRangePlusByFilterQuery, PagedResult<RangePlusResponse>>
{
    private readonly IRangePlusRepository _rangePlusRepository;
    private readonly ILogger<SearchRangePlusByFilterQueryHandler> _logger;

    public SearchRangePlusByFilterQueryHandler(IRangePlusRepository rangePlusRepository, ILogger<SearchRangePlusByFilterQueryHandler> logger)
    {
        _rangePlusRepository = rangePlusRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<PagedResult<RangePlusResponse>>> Handle(SearchRangePlusByFilterQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.FromPlus.HasValue && request.ToPlus.HasValue &&
           request.FromPlus > request.ToPlus)
            {
                return ApiOperationResult.Fail<PagedResult<RangePlusResponse>>(RangePlusError.PlusFilterInvalid());
            }

            var paged = await _rangePlusRepository.SearchByFilterAsync(request, cancellationToken);

            var pagedResult = new PagedResult<RangePlusResponse>
            {
                Currentpage = paged.Currentpage,
                PageSize = paged.PageSize,
                HasNextPage = paged.HasNextPage,
                ItemCount = paged.ItemCount,
                PageCount = paged.PageCount,
                TotalItemCount = paged.TotalItemCount,
                Results = paged.Results?.Select(rp => new RangePlusResponse
                {
                    Id = rp.Id,
                    MaxExperience = rp.MaxExperience,
                    MinExperience = rp.MinExperience,
                    Plus = rp.Plus,
                }).ToList()
            };

            return ApiOperationResult.Success(pagedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<PagedResult<RangePlusResponse>>(ApiOperationError.Failure(ex.GetType().ToString(), ex.Message));
        }
    }
}


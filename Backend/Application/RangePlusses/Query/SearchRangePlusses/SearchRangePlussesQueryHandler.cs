using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Models.RangePlus;
using SharedLib.Models.Common;

namespace Application.RangePlusses.Query.SearchRangePlusses;

internal sealed class SearchRangePlussesQueryHandler : IQueryHandler<SearchRangePlussesQuery, IEnumerable<RangePlusResponse>>
{
    private readonly IRangePlusRepository _rangePlusRepository;
    private readonly ILogger<SearchRangePlussesQueryHandler> _logger;

    public SearchRangePlussesQueryHandler(IRangePlusRepository rangePlusRepository, ILogger<SearchRangePlussesQueryHandler> logger)
    {
        _rangePlusRepository = rangePlusRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<IEnumerable<RangePlusResponse>>> Handle(SearchRangePlussesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var rangeListDb = await _rangePlusRepository.GetAllAsync(cancellationToken);

            List<RangePlusResponse> rangeListResult = new();

            if (rangeListDb.Any())
            {
                rangeListResult.AddRange(
                    rangeListDb.Select(r => new RangePlusResponse
                    {
                        Id = r.Id,
                        MinExperience = r.MinExperience,
                        MaxExperience = r.MaxExperience,
                        Plus = r.Plus
                    }
                 ));
            }

            return ApiOperationResult.Success<IEnumerable<RangePlusResponse>>(rangeListResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<IEnumerable<RangePlusResponse>>(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }


    }
}



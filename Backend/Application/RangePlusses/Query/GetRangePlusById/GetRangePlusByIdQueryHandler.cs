using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Shared.Models.RangePlus;
using SharedLib.Models.Common;

namespace Application.RangePlusses.Query.GetRangePlusById;

internal sealed class GetRangePlusByIdQueryHandler : IQueryHandler<GetRangePlusByIdQuery, RangePlusResponse>
{
    private readonly IRangePlusRepository _rangePlusRepository;
    private readonly ILogger<GetRangePlusByIdQueryHandler> _logger;

    public GetRangePlusByIdQueryHandler(IRangePlusRepository rangePlusRepository, ILogger<GetRangePlusByIdQueryHandler> logger)
    {
        _rangePlusRepository = rangePlusRepository;
        _logger = logger;
    }


    public async Task<ApiOperationResult<RangePlusResponse>> Handle(GetRangePlusByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.Id <= 0)
            {
                return ApiOperationResult.Fail<RangePlusResponse>(RangePlusError.InvalidId(request?.Id ?? 0));
            }

            var rangePlusDb = await _rangePlusRepository.GetByIdAsync(request.Id);

            RangePlusResponse? resultPayload = rangePlusDb is null ?
                new RangePlusResponse
                {
                    Id = rangePlusDb!.Id,
                    MaxExperience = rangePlusDb!.MaxExperience,
                    MinExperience = rangePlusDb!.MinExperience,
                    Plus = rangePlusDb!.Plus
                } : null;

            return ApiOperationResult.Success(resultPayload);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<RangePlusResponse>(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}




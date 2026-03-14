using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Instruments.Queries.GetMostUsedInstruments;

internal sealed class GetMostUsedInstrumentQueryHandler : IQueryHandler<GetMostUsedInstrumentQuery, MostUsedInstrumentResponse>
{
    private readonly IInstrumentRepository _instrumentRepository;
    private readonly ILogger<GetMostUsedInstrumentQueryHandler> _logger;

    public GetMostUsedInstrumentQueryHandler(IInstrumentRepository repository, ILogger<GetMostUsedInstrumentQueryHandler> logger)
    {
        _instrumentRepository = repository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<MostUsedInstrumentResponse>> Handle(GetMostUsedInstrumentQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.InstrumentQtyToSearch == 0)
            {
                return ApiOperationResult.Fail<MostUsedInstrumentResponse>(InstrumentError.InvalidInstrumentQtyToSearch());
            }

            var response = await _instrumentRepository.GetMostUsedInstrument(request.InstrumentQtyToSearch, cancellationToken);

            return ApiOperationResult.Success<MostUsedInstrumentResponse>(new MostUsedInstrumentResponse(response));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<MostUsedInstrumentResponse>(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}

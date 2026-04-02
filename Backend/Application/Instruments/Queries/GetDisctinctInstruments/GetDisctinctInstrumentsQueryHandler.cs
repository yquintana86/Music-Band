using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Instruments.Queries.GetDisctinctInstruments;

internal sealed class GetDisctinctInstrumentsQueryHandler : IQueryHandler<DisctinctInstrumentsQuery, IEnumerable<SelectItem>>
{
    private readonly IInstrumentRepository _instrumentRepository;
    private readonly ILogger<GetDisctinctInstrumentsQueryHandler> _logger;
    public GetDisctinctInstrumentsQueryHandler(IInstrumentRepository instrumentRepository, ILogger<GetDisctinctInstrumentsQueryHandler> logger)
    {
        _instrumentRepository = instrumentRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<IEnumerable<SelectItem>>> Handle(DisctinctInstrumentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _instrumentRepository.GetDisctinctAsync(cancellationToken);
            return ApiOperationResult.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return ApiOperationResult.Fail<IEnumerable<SelectItem>>(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}

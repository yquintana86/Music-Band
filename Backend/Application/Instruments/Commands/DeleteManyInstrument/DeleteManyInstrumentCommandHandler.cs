using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Instruments.Commands.DeleteManyInstrument;

internal sealed class DeleteManyInstrumentCommandHandler : ICommandHandler<DeleteManyInstrumentCommand>
{
    private readonly IInstrumentRepository _instrumentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteManyInstrumentCommandHandler> _logger;

    public DeleteManyInstrumentCommandHandler(IInstrumentRepository instrumentRepository, IUnitOfWork unitOfWork, ILogger<DeleteManyInstrumentCommandHandler> logger)
    {
        _instrumentRepository = instrumentRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApiOperationResult> Handle(DeleteManyInstrumentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _instrumentRepository.DeleteManyAsync(request.instrumentIds);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiOperationResult.Success();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return ApiOperationResult.Fail(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}

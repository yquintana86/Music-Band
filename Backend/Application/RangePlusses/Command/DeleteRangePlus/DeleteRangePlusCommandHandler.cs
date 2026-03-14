using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.RangePlusses.Command.DeleteRangePlus;

internal sealed class DeleteRangePlusCommandHandler : ICommandHandler<DeleteRangePlusCommand>
{
    private readonly IRangePlusRepository _rangePlusRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteRangePlusCommandHandler> _logger;

    public DeleteRangePlusCommandHandler(IRangePlusRepository rangePlusRepository, IUnitOfWork unitOfWork, ILogger<DeleteRangePlusCommandHandler> logger)
    {
        _rangePlusRepository = rangePlusRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApiOperationResult> Handle(DeleteRangePlusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var ranglePlusDb = await _rangePlusRepository.GetByIdAsync(request.Id);

            if (ranglePlusDb is null)
            {
                return ApiOperationResult.Fail(RangePlusError.NotFound(request.Id));
            }

            await _rangePlusRepository.DeleteAsync(request.Id, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}


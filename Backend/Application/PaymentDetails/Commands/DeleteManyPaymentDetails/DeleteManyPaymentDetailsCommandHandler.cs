
using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.PaymentDetails.Commands.DeleteManyPaymentDetails;

public sealed class DeleteManyPaymentDetailsCommandHandler : ICommandHandler<DeleteManyPaymentDetailsCommand>
{
    private readonly IMusicianPaymentDetailsRepository _musicianPaymentDetailsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteManyPaymentDetailsCommandHandler> _logger;

    public DeleteManyPaymentDetailsCommandHandler(IMusicianPaymentDetailsRepository musicianPaymentDetailsRepository, IUnitOfWork unitOfWork, ILogger<DeleteManyPaymentDetailsCommandHandler> logger)
    {
        _musicianPaymentDetailsRepository = musicianPaymentDetailsRepository;
        this._unitOfWork = unitOfWork;
        _logger = logger;
    }


    public async Task<ApiOperationResult> Handle(DeleteManyPaymentDetailsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _musicianPaymentDetailsRepository.DeleteManyAsync(request.Ids, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiOperationResult.Success();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return ApiOperationResult.Fail(new ApiOperationError(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure));
        }
    }
}


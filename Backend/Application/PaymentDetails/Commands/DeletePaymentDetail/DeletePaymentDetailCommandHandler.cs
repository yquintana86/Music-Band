
using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.PaymentDetails.Commands.DeletePaymentDetail;

internal sealed class DeletePaymentDetailCommandHandler : ICommandHandler<DeletePaymentDetailCommand>
{
    private readonly IMusicianPaymentDetailsRepository _musicianPaymentDetailsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeletePaymentDetailCommandHandler> _logger;
    public DeletePaymentDetailCommandHandler(IMusicianPaymentDetailsRepository musicianPaymentDetailsRepository, ILogger<DeletePaymentDetailCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _musicianPaymentDetailsRepository = musicianPaymentDetailsRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async  Task<ApiOperationResult> Handle(DeletePaymentDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var paymentDetailDb = await _musicianPaymentDetailsRepository.GetByIdAsync(request.Id);
            if (paymentDetailDb is null)
            {
                return ApiOperationResult.Fail(PaymentDetailError.PaymentNotFound());
            }

            await _musicianPaymentDetailsRepository.DeleteAsync(request.Id);
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

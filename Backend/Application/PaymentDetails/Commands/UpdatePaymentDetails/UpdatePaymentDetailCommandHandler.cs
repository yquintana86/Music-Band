using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.PaymentDetails.Commands.UpdatePaymentDetails;

internal sealed class UpdatePaymentDetailCommandHandler : ICommandHandler<UpdatePaymentDetailCommand>
{
    private readonly IMusicianPaymentDetailsRepository _musicianPaymentDetailsRepository;
    private readonly IMusicianRepository _musicianRepository;
    private readonly IRangePlusRepository _rangePlusRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdatePaymentDetailCommandHandler> _logger;


    public UpdatePaymentDetailCommandHandler(IMusicianPaymentDetailsRepository musicianPaymentDetailsRepository, ILogger<UpdatePaymentDetailCommandHandler> logger, IRangePlusRepository rangePlusRepository, IUnitOfWork unitOfWork, IMusicianRepository musicianRepository)
    {
        _musicianPaymentDetailsRepository = musicianPaymentDetailsRepository;
        _musicianRepository = musicianRepository;
        _rangePlusRepository = rangePlusRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApiOperationResult> Handle(UpdatePaymentDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var paymentDetailDb = await _musicianPaymentDetailsRepository.GetByIdAsync(request.Id, cancellationToken);
            if (paymentDetailDb is null)
            {
                return ApiOperationResult.Fail(PaymentDetailError.PaymentNotFound());
            }

            var musicianDb = await _musicianRepository.GetByIdAsync(request.MusicianId, cancellationToken);
            if (musicianDb is null)
            {
                return ApiOperationResult.Fail(PaymentDetailError.PaymentWithMusicianNotFound());
            }

            var rangePlusDb = await _rangePlusRepository.GetByIdAsync(request.RangePlusId, cancellationToken);
            if (rangePlusDb is null)
            {
                return ApiOperationResult.Fail(PaymentDetailError.PaymentWithRelatedPlusNotFound());
            }

            paymentDetailDb.MusicianId = request.MusicianId;
            paymentDetailDb.Salary = request.Salary;
            paymentDetailDb.BasicSalary = request.BasicSalary;
            paymentDetailDb.RangePlusId = request.RangePlusId;
            paymentDetailDb.PaymentDate = request.PaymentDate;

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

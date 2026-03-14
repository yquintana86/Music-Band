using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;
using System.Linq.Expressions;

namespace Application.PaymentDetails.Commands.CreatePaymentDetails;

internal sealed class CreatePaymentDetailCommandHandler : ICommandHandler<CreatePaymentDetailCommand>
{
    private readonly IMusicianPaymentDetailsRepository _musicianPaymentDetailsRepository;
    private readonly IMusicianRepository _musicianRepository;
    private readonly IRangePlusRepository _rangePlusRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreatePaymentDetailCommandHandler> _logger;

    public CreatePaymentDetailCommandHandler(IMusicianPaymentDetailsRepository musicianPaymentDetailsRepository, IUnitOfWork unitOfWork, ILogger<CreatePaymentDetailCommandHandler> logger, IRangePlusRepository rangePlusRepository, IMusicianRepository musicianRepository)
    {
        _musicianPaymentDetailsRepository = musicianPaymentDetailsRepository;
        _musicianRepository = musicianRepository;
        _rangePlusRepository = rangePlusRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApiOperationResult> Handle(CreatePaymentDetailCommand request, CancellationToken cancellationToken)
    {
        try
        {
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

            var musicianPaymentDetail = new MusicianPaymentDetail
            {
                PaymentDate = request.PaymentDate,
                BasicSalary = request.BasicSalary,
                MusicianId = request.MusicianId,
                RangePlusId = request.RangePlusId,
                Salary = request.Salary
            };

            _musicianPaymentDetailsRepository.Add(musicianPaymentDetail);
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



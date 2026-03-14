using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Shared.Models.PaymentDetail;
using SharedLib.Models.Common;

namespace Application.PaymentDetails.Queries.GetPaymentDetailById;

internal sealed class GetPaymentDetailByIdQueryHandler : IQueryHandler<GetPaymentDetailByIdQuery, PaymentDetailResponse?>
{
    private readonly IMusicianPaymentDetailsRepository _musicianPaymentDetailsRepository;
    private readonly ILogger<GetPaymentDetailByIdQueryHandler> _logger;

    public GetPaymentDetailByIdQueryHandler(IMusicianPaymentDetailsRepository musicianPaymentDetailsRepository, ILogger<GetPaymentDetailByIdQueryHandler> logger)
    {
        _musicianPaymentDetailsRepository = musicianPaymentDetailsRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<PaymentDetailResponse?>> Handle(GetPaymentDetailByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || request.Id <= 0)
            {
                return ApiOperationResult.Fail<PaymentDetailResponse?>(PaymentDetailError.PaymentNotFound());
            }


            var paymentDetailDb = await _musicianPaymentDetailsRepository.GetByIdAsync(request.Id);

            return ApiOperationResult.Success(paymentDetailDb is not null ?
             new PaymentDetailResponse
             {
                 Id = paymentDetailDb!.Id,
                 BasicSalary = paymentDetailDb!.BasicSalary,
                 MusicianId = paymentDetailDb!.MusicianId,
                 PaymentDate = paymentDetailDb!.PaymentDate,
                 RangePlusId = paymentDetailDb!.RangePlusId,
                 Salary = paymentDetailDb!.Salary
             } : null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<PaymentDetailResponse?>(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}

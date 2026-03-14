using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.PaymentDetails.Queries.GetAgeAvgExceedingSalary;

internal sealed class GetAgeAvgExceedingSalaryQueryHandler : IQueryHandler<GetAgeAvgExceedingSalaryQuery, decimal>
{

    private readonly IMusicianPaymentDetailsRepository _musicianPaymentDetailsRepository;
    private readonly ILogger<GetAgeAvgExceedingSalaryQueryHandler> _logger;

    public GetAgeAvgExceedingSalaryQueryHandler(IMusicianPaymentDetailsRepository musicianPaymentDetailsRepository, ILogger<GetAgeAvgExceedingSalaryQueryHandler> logger)
    {
        _musicianPaymentDetailsRepository = musicianPaymentDetailsRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<decimal>> Handle(GetAgeAvgExceedingSalaryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.salary <= 0)
            {
                return ApiOperationResult.Fail<decimal>(PaymentDetailError.PaymentSalaryFilterInvalid());
            }

            var (avg, _) = await _musicianPaymentDetailsRepository.GetAgeAvgExceedingSalary(request.salary, cancellationToken);

            return ApiOperationResult.Success(avg);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<decimal>(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}

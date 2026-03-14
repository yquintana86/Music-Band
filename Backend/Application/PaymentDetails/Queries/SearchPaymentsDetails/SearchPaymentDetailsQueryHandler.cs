using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Models.PaymentDetail;
using SharedLib.Models.Common;

namespace Application.PaymentDetails.Queries.SearchPaymentsDetails;

internal sealed class SearchPaymentDetailsQueryHandler : IQueryHandler<SearchPaymentDetailsQuery, IEnumerable<PaymentDetailResponse>>
{

    private readonly IMusicianPaymentDetailsRepository _musicianPaymentDetailsRepository;
    private readonly ILogger<SearchPaymentDetailsQueryHandler> _logger;

    public SearchPaymentDetailsQueryHandler(IMusicianPaymentDetailsRepository musicianPaymentDetailsRepository, ILogger<SearchPaymentDetailsQueryHandler> logger)
    {
        _musicianPaymentDetailsRepository = musicianPaymentDetailsRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<IEnumerable<PaymentDetailResponse>>> Handle(SearchPaymentDetailsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var paymentDetailListDb = await _musicianPaymentDetailsRepository.GetAllAsync();

            var paymentDetailListResult = new List<PaymentDetailResponse>();

            if (paymentDetailListDb.Any())
            {
                paymentDetailListResult.AddRange(paymentDetailListDb.Select(pd => new PaymentDetailResponse
                {
                    Id = pd.Id,
                    PaymentDate = pd.PaymentDate,
                    Salary = pd.Salary,
                    BasicSalary = pd.BasicSalary,
                    MusicianId = pd.MusicianId,
                    RangePlusId = pd.RangePlusId,
                }));
            }

            return ApiOperationResult.Success<IEnumerable<PaymentDetailResponse>>(paymentDetailListResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<IEnumerable<PaymentDetailResponse>>(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}

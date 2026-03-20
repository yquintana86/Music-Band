using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Shared.Models.PaymentDetail;
using SharedLib.Models.Common;

namespace Application.PaymentDetails.Queries.SearchPaymentByFilter;

internal sealed class SearchPaymentDetailsByFilterQueryHandler : IQueryHandler<SearchPaymentDetailsByFilterQuery, PagedResult<PaymentDetailResponse>>
{

    private readonly IMusicianPaymentDetailsRepository _musicianPaymentDetailsRepository;
    private readonly ILogger<SearchPaymentDetailsByFilterQueryHandler> _logger;

    public SearchPaymentDetailsByFilterQueryHandler(IMusicianPaymentDetailsRepository musicianPaymentDetailsRepository, ILogger<SearchPaymentDetailsByFilterQueryHandler> logger)
    {
        _musicianPaymentDetailsRepository = musicianPaymentDetailsRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<PagedResult<PaymentDetailResponse>>> Handle(SearchPaymentDetailsByFilterQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.FromPaymentDate.HasValue && request.ToPaymentDate.HasValue &&
           request.FromPaymentDate > request.ToPaymentDate)
            {
                return ApiOperationResult.Fail<PagedResult<PaymentDetailResponse>>(PaymentDetailError.PaymentDateFilterInvalid());
            }

            if (request.FromSalary.HasValue && request.ToSalary.HasValue &&
                request.FromSalary > request.ToSalary)
            {
                return ApiOperationResult.Fail<PagedResult<PaymentDetailResponse>>(PaymentDetailError.PaymentSalaryFilterInvalid());
            }

            var paged = await _musicianPaymentDetailsRepository.SearchByFilterAsync(request, cancellationToken);

            var pagedResult = new PagedResult<PaymentDetailResponse>
            {
                Currentpage = paged.Currentpage,
                HasNextPage = paged.HasNextPage,
                ItemCount = paged.ItemCount,
                PageCount = paged.PageCount,
                PageSize = paged.PageSize,
                TotalItemCount = paged.TotalItemCount,
                Results = paged.Results?.Select(pd => new PaymentDetailResponse
                {
                    Id = pd.Id,
                    BasicSalary = pd.BasicSalary,
                    MusicianId = pd.MusicianId,
                    MusicianName = $"{pd.Musician.FirstName} {pd.Musician.LastName}",
                    PaymentDate = pd.PaymentDate,
                    RangePlusId = pd.RangePlusId,
                    Salary = pd.Salary,
                }).ToList()
            };

            return ApiOperationResult.Success(pagedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<PagedResult<PaymentDetailResponse>>(ApiOperationError.Failure(ex.GetType().ToString(), ex.Message));
        }


    }

}

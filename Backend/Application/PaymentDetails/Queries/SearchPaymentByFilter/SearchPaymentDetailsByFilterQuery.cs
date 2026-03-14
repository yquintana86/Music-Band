using Application.Abstractions.Messaging;
using Domain.Entities;
using Shared.Models.PaymentDetail;
using SharedLib.Models.Common;

namespace Application.PaymentDetails.Queries.SearchPaymentByFilter;

public sealed class SearchPaymentDetailsByFilterQuery : PagingFilter,  IQuery<PagedResult<PaymentDetailResponse>>
{
    public DateTime? FromPaymentDate { get; set; }
    public DateTime? ToPaymentDate { get; set; }
    public decimal? FromSalary { get; set; }
    public decimal? ToSalary { get; set; }
}

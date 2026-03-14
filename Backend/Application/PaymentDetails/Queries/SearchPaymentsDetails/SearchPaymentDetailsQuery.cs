using Application.Abstractions.Messaging;
using Shared.Models.PaymentDetail;

namespace Application.PaymentDetails.Queries.SearchPaymentsDetails;

public sealed record SearchPaymentDetailsQuery : IQuery<IEnumerable<PaymentDetailResponse>>;
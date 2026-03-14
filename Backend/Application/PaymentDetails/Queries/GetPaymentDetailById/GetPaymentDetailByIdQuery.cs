using Application.Abstractions.Messaging;
using Shared.Models.PaymentDetail;

namespace Application.PaymentDetails.Queries.GetPaymentDetailById;

public sealed record GetPaymentDetailByIdQuery(int Id) : IQuery<PaymentDetailResponse?>;
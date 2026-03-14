
using Application.Abstractions.Messaging;
using System.Runtime.CompilerServices;

namespace Application.PaymentDetails.Commands.DeletePaymentDetail;

public sealed record DeletePaymentDetailCommand(int Id) : ICommand;

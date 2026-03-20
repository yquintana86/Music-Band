using Application.Abstractions.Messaging;
using Domain.Exceptions;
using System.Drawing;

namespace Application.PaymentDetails.Commands.DeleteManyPaymentDetails;

public sealed record DeleteManyPaymentDetailsCommand(List<int> Ids) : ICommand;

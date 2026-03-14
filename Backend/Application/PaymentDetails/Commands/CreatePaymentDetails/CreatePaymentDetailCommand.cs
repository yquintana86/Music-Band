using Application.Abstractions.Messaging;

namespace Application.PaymentDetails.Commands.CreatePaymentDetails;

public sealed record CreatePaymentDetailCommand(DateTime PaymentDate, decimal Salary, decimal BasicSalary, int MusicianId, int RangePlusId) : ICommand;
using Application.Abstractions.Messaging;

namespace Application.PaymentDetails.Commands.UpdatePaymentDetails;

public sealed record UpdatePaymentDetailCommand(int Id, DateTime PaymentDate, decimal Salary, decimal BasicSalary, int MusicianId, int RangePlusId) : ICommand;
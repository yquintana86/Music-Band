using Application.Abstractions.Messaging;
using System.Reflection.Metadata;

namespace Application.PaymentDetails.Commands.UpdatePaymentDetails;

public sealed record UpdatePaymentDetailCommand(int Id, DateTime PaymentDate, decimal Salary, decimal BasicSalary, int MusicianId, int RangePlusId, string? description) : ICommand;
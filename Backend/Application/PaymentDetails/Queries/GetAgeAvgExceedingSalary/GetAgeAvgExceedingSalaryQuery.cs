using Application.Abstractions.Messaging;

namespace Application.PaymentDetails.Queries.GetAgeAvgExceedingSalary;

public sealed record GetAgeAvgExceedingSalaryQuery(decimal salary) : IQuery<decimal>;
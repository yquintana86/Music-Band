using FluentValidation;

namespace Application.PaymentDetails.Commands.CreatePaymentDetails;

public sealed class CreatePaymentDetailCommandValidator : AbstractValidator<CreatePaymentDetailCommand>
{
    public CreatePaymentDetailCommandValidator()
    {
        RuleFor(pd => pd.PaymentDate).Custom((pd, context) =>
        {
            if (pd.Year != DateTime.UtcNow.Year)
                context.AddFailure("A Payment must be done in the current year");

            if (pd.Month != DateTime.UtcNow.Month)
                context.AddFailure("A Payment must be done in the current month");
        });

        RuleFor(pd => pd.Salary).GreaterThan(0);
        RuleFor(pd => pd.BasicSalary).GreaterThan(0);
        RuleFor(pd => pd.MusicianId).GreaterThan(0);     
        RuleFor(pd => pd.RangePlusId).GreaterThan(0);    
    }
}



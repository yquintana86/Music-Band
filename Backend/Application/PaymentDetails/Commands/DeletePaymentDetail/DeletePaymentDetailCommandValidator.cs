using FluentValidation;

namespace Application.PaymentDetails.Commands.DeletePaymentDetail;

public class DeletePaymentDetailCommandValidator : AbstractValidator<DeletePaymentDetailCommand>
{
    public DeletePaymentDetailCommandValidator()
    {
            RuleFor(pd => pd.Id).GreaterThan(0);
    }
}
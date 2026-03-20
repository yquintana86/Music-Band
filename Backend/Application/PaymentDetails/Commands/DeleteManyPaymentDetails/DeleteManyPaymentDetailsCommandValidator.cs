using FluentValidation;

namespace Application.PaymentDetails.Commands.DeleteManyPaymentDetails;

public sealed class DeleteManyPaymentDetailsCommandValidator : AbstractValidator<DeleteManyPaymentDetailsCommand>
{
    public DeleteManyPaymentDetailsCommandValidator()
    {
        RuleFor(dm => dm).NotNull();
        RuleFor(dm => dm.Ids).NotEmpty();
    }
}


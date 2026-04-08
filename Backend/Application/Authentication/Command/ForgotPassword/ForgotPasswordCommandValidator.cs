using FluentValidation;
using Shared.Common;

namespace Application.Authentication.Command.ForgotPassword;

public sealed class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(p => p.email).NotEmpty();
        RuleFor(p => p.email).Custom((email, context) =>
        {
            if (!Email.IsValid(email))
            {
                context.AddFailure($"The email: {email} is not a valid email");
            }
        });
    }
}



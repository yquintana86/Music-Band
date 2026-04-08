using FluentValidation;
using Shared.Common;

namespace Application.Authentication.Command.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(rp => rp.token).NotEmpty();
        RuleFor(rp => rp.password).NotEmpty();
        RuleFor(rp => rp.email).Custom((email, validatioctx) => {

            if (!Email.IsValid(email))
            {
                validatioctx.AddFailure("Invalid Email Format");
            }

        });
    }
}


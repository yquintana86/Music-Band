using FluentValidation;
using Shared.Common;

namespace Application.Authentication.Command.ResetForgottenPassword;

public class ResetForgottenPasswordCommandValidator : AbstractValidator<ResetForgottenPassword>
{
    public ResetForgottenPasswordCommandValidator()
    {
        RuleFor(rp => rp.token).NotEmpty();
        RuleFor(rp => rp.password).NotEmpty();
        RuleFor(rp => rp.email).Custom((email, validatioctx) =>
        {

            if (!Email.IsValid(email))
            {
                validatioctx.AddFailure("Invalid Email Format");
            }

        });
    }
}


using Application.Abstractions.Messaging;

namespace Application.Authentication.Command.ForgotPassword;

public sealed record ForgotPasswordCommand(string email): ICommand;
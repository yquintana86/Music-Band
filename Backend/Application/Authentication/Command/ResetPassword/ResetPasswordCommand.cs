using Application.Abstractions.Messaging;

namespace Application.Authentication.Command.ResetPassword;

public sealed record ResetPasswordCommand(string email, string password, string token): ICommand;
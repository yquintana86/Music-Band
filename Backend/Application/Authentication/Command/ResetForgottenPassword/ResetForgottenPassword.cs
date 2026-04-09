using Application.Abstractions.Messaging;

namespace Application.Authentication.Command.ResetForgottenPassword;

public sealed record ResetForgottenPassword(string email, string password, string token) : ICommand;
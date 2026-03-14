using Application.Abstractions.Messaging;

namespace Application.Authentication.Command.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<CredentialsResponse>;




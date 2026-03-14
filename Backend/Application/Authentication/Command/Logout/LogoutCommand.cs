using Application.Abstractions.Messaging;

namespace Application.Authentication.Command.Logout;
public record LogoutCommand(string? sub, string refreshToken) : ICommand;

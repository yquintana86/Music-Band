using Application.Abstractions.Messaging;

namespace Application.Authentication.Command.RefreshToken;

public record RefreshTokenCommand(int userId, string refreshtoken) : ICommand<CredentialsResponse>;


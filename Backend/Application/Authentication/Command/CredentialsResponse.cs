namespace Application.Authentication.Command;

public record CredentialsResponse(string accessToken, string refreshToken);


using Application.Abstractions.Messaging;

namespace Application.Authentication.Command.CreateUser;

public sealed record RegisterUserCommand(string Email, string Password, string FirstName, string? LastName) : ICommand;

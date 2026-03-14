using Application.Abstractions.Messaging;

namespace Application.Musicians.Command.CreateMusician;

public record CreateMusicianCommand(string FirstName, string? MiddleName, string LastName, int Age, int Experience, double BasicSalary) : ICommand;

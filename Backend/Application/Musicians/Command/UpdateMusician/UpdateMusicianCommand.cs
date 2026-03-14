using Application.Abstractions.Messaging;

namespace Application.Musicians.Command.UpdateMusician;

public sealed record UpdateMusicianCommand(int Id, string FirstName, string? MiddleName, string LastName, int Age, int Experience, decimal BasicSalary) : ICommand;



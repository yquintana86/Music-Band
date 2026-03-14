using Application.Abstractions.Messaging;

namespace Application.Musicians.Command.DeleteMusician;

public sealed record DeleteMusicianCommand(int Id) : ICommand;

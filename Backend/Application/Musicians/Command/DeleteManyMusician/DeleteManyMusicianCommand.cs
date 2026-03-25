using Application.Abstractions.Messaging;

namespace Application.Musicians.Command.DeleteManyMusician;

public sealed record DeleteManyMusicianCommand(List<int> ids) : ICommand;
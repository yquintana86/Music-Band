using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Musicians.Command.DeleteMusician;

internal sealed class DeleteMusicianCommandHandler : ICommandHandler<DeleteMusicianCommand>
{

    private readonly IMusicianRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteMusicianCommandHandler> _logger;

    public DeleteMusicianCommandHandler(IMusicianRepository repository, ILogger<DeleteMusicianCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiOperationResult> Handle(DeleteMusicianCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var exist = await _repository.ExistIdAsync(request.Id,cancellationToken);
            if (!exist)
                return ApiOperationResult.Fail(MusicianError.NotFound(request.Id));

            await _repository.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail(ex.GetType().Name, ex.Message, ApiErrorType.Failure);
        }
    }
}

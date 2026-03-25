using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Musicians.Command.DeleteManyMusician;

internal sealed class DeleteManyMusicianCommandHanlder : ICommandHandler<DeleteManyMusicianCommand>
{
    private readonly IMusicianRepository _musicianRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteManyMusicianCommandHanlder> _logger;
    public DeleteManyMusicianCommandHanlder(IMusicianRepository musicianRepository, IUnitOfWork unitOfWork, ILogger<DeleteManyMusicianCommandHanlder> logger)
    {
        _musicianRepository = musicianRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApiOperationResult> Handle(DeleteManyMusicianCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _musicianRepository.DeleteManyAsync(request.ids, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return ApiOperationResult.Fail(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}

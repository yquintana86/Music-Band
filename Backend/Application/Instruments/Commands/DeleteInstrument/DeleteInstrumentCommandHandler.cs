
using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Instrument.Commands.DeleteInstrument;

internal sealed class DeleteInstrumentCommandHandler : ICommandHandler<DeleteInstrumentCommand>
{

    private readonly IInstrumentRepository _instrumentRepository;
    private readonly IMusicianRepository _musicianRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteInstrumentCommandHandler> _logger;

    public DeleteInstrumentCommandHandler(IInstrumentRepository instrumentRepository, ILogger<DeleteInstrumentCommandHandler> logger, IMusicianRepository musicianRepository, IUnitOfWork unitOfWork)
    {
        _instrumentRepository = instrumentRepository;
        _logger = logger;
        _musicianRepository = musicianRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiOperationResult> Handle(DeleteInstrumentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var musicianInstrumentDb = await _instrumentRepository.GetByIdAsync(request.Id);
            
            if (musicianInstrumentDb is null)
                return ApiOperationResult.Fail(InstrumentError.NotFound());

            var owner = await _musicianRepository.GetByIdWithRelatedEntitiesAsync(musicianInstrumentDb.MusicianId, m => m.Instruments, cancellationToken);
            if (owner is null)
                return ApiOperationResult.Fail(InstrumentError.OwnerNotFound(owner?.Id ?? 0));

            if (owner.Instruments.Count <= 1)
                return ApiOperationResult.Fail(
                    InstrumentError.LastInstrumentToDelete(
                        string.Join(" ", owner.FirstName, owner.MiddleName, owner.LastName), musicianInstrumentDb.Name));

            await _instrumentRepository.DeleteAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error has happened {}", ex.Message);
            return ApiOperationResult.Fail(ApiOperationError.Failure(ex.GetType().Name, ex.Message));
        }
    }
}

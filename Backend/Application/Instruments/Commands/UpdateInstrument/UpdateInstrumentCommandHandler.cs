using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Instrument.Commands.UpdateInstrument;

internal sealed class UpdateInstrumentCommandHandler : ICommandHandler<UpdateInstrumentCommand>
{
    private readonly IInstrumentRepository _instrumentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMusicianRepository _musicianRepository;
    private readonly ILogger<UpdateInstrumentCommandHandler> _logger;

    public UpdateInstrumentCommandHandler(IInstrumentRepository instrumentRepository, IMusicianRepository musicianRepository, ILogger<UpdateInstrumentCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _instrumentRepository = instrumentRepository;
        _musicianRepository = musicianRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiOperationResult> Handle(UpdateInstrumentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var instrumentDb = await this._instrumentRepository.GetByIdAsync(request.Id);

            if (instrumentDb == null)
                return ApiOperationResult.Fail(InstrumentError.NotFound());

            var musician = await _musicianRepository.GetByIdWithRelatedEntitiesAsync(request.MusicianId, m => m.Instruments, cancellationToken);

            if (musician is null)
                return ApiOperationResult.Fail(InstrumentError.OwnerNotFound(request.MusicianId));

            if(musician.Instruments.Any(i => i.Name.ToLower() == request.Name.ToLower()))
                return ApiOperationResult.Fail(InstrumentError.DuplicateOwner());

            instrumentDb!.Name = request.Name;
            instrumentDb!.Country = request.Country;
            instrumentDb!.Description = request.Description;
            instrumentDb!.MusicianId = request.MusicianId;
            instrumentDb!.Type = request.Type;

            await _unitOfWork.SaveChangesAsync();

            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has happened {}", ex.Message);
            return ApiOperationResult.Fail(ApiOperationError.Failure(ex.GetType().Name, ex.Message));
        }
    }
}

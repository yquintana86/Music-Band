using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Instrument.Commands.CreateInstrument;

internal sealed class CreateInstrumentCommandHandler : ICommandHandler<CreateInstrumentCommand>
{
    private readonly IInstrumentRepository _instrumentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMusicianRepository _musicianRepository;
    private readonly ILogger<CreateInstrumentCommandHandler> _logger;

    public CreateInstrumentCommandHandler(IInstrumentRepository instrumentRepository, IMusicianRepository musicianRepository, 
        ILogger<CreateInstrumentCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _instrumentRepository = instrumentRepository;
        _musicianRepository = musicianRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiOperationResult> Handle(CreateInstrumentCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var musician = await _musicianRepository.GetByIdAsync(request.MusicianId,cancellationToken);

            if (musician is null)
                return ApiOperationResult.Fail(InstrumentError.OwnerNotFound(request.MusicianId));

            var instrument = new Domain.Entities.MusicalInstrument
            {
                Name = request.Name,
                Country = request.Country,
                Description = request.Description,
                MusicianId = request.MusicianId,
                Type = request.Type,
            };

            _instrumentRepository.Add(instrument);
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


using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Musicians.Command.UpdateMusician;

internal sealed class UpdateMusicianCommandHandler : ICommandHandler<UpdateMusicianCommand>
{
    private readonly IMusicianRepository _musicianRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateMusicianCommandHandler> _logger;


    public UpdateMusicianCommandHandler(IMusicianRepository musicianRepository, ILogger<UpdateMusicianCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _musicianRepository = musicianRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiOperationResult> Handle(UpdateMusicianCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var musician = await _musicianRepository.GetByIdAsync(request.Id);

            if (musician is null)
                return ApiOperationResult.Fail(MusicianError.NotFound(request.Id));

            musician.FirstName = musician.FirstName;
            musician.MiddleName = musician.MiddleName;
            musician.LastName = musician.LastName;
            musician.Age = musician.Age;
            musician.Experience = musician.Experience;
            musician.Activities = musician.Activities;
            musician.Instruments = musician.Instruments;
            musician.BasicSalary = musician.BasicSalary;

            await _unitOfWork.SaveChangesAsync();
            return ApiOperationResult.Success();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail(ex.GetType().Name, ex.Message, ApiErrorType.Failure);
        }
    }
}



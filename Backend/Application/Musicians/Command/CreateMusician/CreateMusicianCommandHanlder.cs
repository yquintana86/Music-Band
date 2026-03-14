using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.Musicians.Command.CreateMusician;

internal sealed class CreateMusicianCommandHandler : ICommandHandler<CreateMusicianCommand>
{
    private readonly IMusicianRepository _musicianRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateMusicianCommandHandler> _logger;

    public CreateMusicianCommandHandler(IMusicianRepository musicianRepository, ILogger<CreateMusicianCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _musicianRepository = musicianRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiOperationResult> Handle(CreateMusicianCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var musician = new Musician
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Age = request.Age,
                Experience = request.Experience,
                BasicSalary = request.BasicSalary,
            };

            _musicianRepository.Add(musician);
            await _unitOfWork.SaveChangesAsync();

            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail(ex.GetType().Name,ex.Message,ApiErrorType.Failure);

        }
    }
}

using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.RangePlusses.Command.CreateRangePlus;

internal sealed class CreateRangePlusCommandHandler : ICommandHandler<CreateRangePlusCommand>
{
    private readonly IRangePlusRepository _rangePlusRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateRangePlusCommandHandler> _logger;

    public CreateRangePlusCommandHandler(IRangePlusRepository rangePlusRepository, ILogger<CreateRangePlusCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _rangePlusRepository = rangePlusRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiOperationResult> Handle(CreateRangePlusCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (await _rangePlusRepository.IsExperienceRangeWithConflictAsync(request.MinExperience, request.MaxExperience, cancellationToken))
            {
                return ApiOperationResult.Fail(RangePlusError.ExperienceConflictWithExistingRanges(request.MinExperience, request.MaxExperience));
            }

            RangePlus ranglePlus = new RangePlus
            {
                MinExperience = request.MinExperience,
                MaxExperience = request.MaxExperience,
                Plus = request.plus
            };

            _rangePlusRepository.Add(ranglePlus);
            await _unitOfWork.SaveChangesAsync();

            return ApiOperationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail(ex.GetType().ToString(), ex.Message, ApiErrorType.Failure);
        }
    }
}


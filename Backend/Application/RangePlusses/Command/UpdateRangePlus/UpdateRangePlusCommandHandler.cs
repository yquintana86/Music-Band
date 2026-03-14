using Application.Abstractions.DataContext;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using SharedLib.Models.Common;

namespace Application.RangePlusses.Command.UpdateRangePlus;

internal sealed class UpdateRangePlusCommandHandler : ICommandHandler<UpdateRangePlusCommand>
{
    private readonly IRangePlusRepository _rangePlusRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateRangePlusCommandHandler> _logger;

    public UpdateRangePlusCommandHandler(IRangePlusRepository rangePlusRepository, ILogger<UpdateRangePlusCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _rangePlusRepository = rangePlusRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApiOperationResult> Handle(UpdateRangePlusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var rangePlusDb = await _rangePlusRepository.GetByIdAsync(request.Id, cancellationToken);

            if (rangePlusDb is null)
            {
                return ApiOperationResult.Fail(RangePlusError.NotFound(request.Id));
            }

            rangePlusDb.MinExperience = request.MinExperience;
            rangePlusDb.MaxExperience = request.MaxExperience;
            rangePlusDb.Plus = request.plus;

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




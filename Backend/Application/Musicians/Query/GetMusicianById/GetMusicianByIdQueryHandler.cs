using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Shared.Models.Musician;
using SharedLib.Models.Common;

namespace Application.Musicians.Query.SearchMusicianById;

internal sealed class GetMusicianByIdQueryHandler : IQueryHandler<GetMusicianByIdQuery, MusicianResponse?>
{
    private readonly IMusicianRepository _musicianRepository;
    private readonly ILogger<GetMusicianByIdQueryHandler> _logger;

    public GetMusicianByIdQueryHandler(IMusicianRepository musicianRepository, ILogger<GetMusicianByIdQueryHandler> logger)
    {
        _musicianRepository = musicianRepository;
        _logger = logger;
    }

    public async Task<ApiOperationResult<MusicianResponse?>> Handle(GetMusicianByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var requestId = request?.Id ?? 0;
            if (requestId == 0)
                return ApiOperationResult.Fail<MusicianResponse?>(MusicianError.InvalidId(requestId));


            var musician = await _musicianRepository.GetByIdAsync(requestId, cancellationToken);

            MusicianResponse? model = null;

            if (musician is not null)
            {
                model = new MusicianResponse
                {
                    Id = musician.Id,
                    FirstName = musician.FirstName,
                    MiddleName = musician.MiddleName,
                    LastName = musician.LastName,
                    Age = musician.Age,
                    Experience = musician.Experience,
                };
            }

            return ApiOperationResult.Success<MusicianResponse?>(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return ApiOperationResult.Fail<MusicianResponse?>(ex.GetType().Name, ex.Message, ApiErrorType.Failure);
        }
    }
}

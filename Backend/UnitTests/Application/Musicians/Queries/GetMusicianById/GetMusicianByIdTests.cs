using Application.Abstractions.Repositories;
using Application.Musicians.Query.SearchMusicianById;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SharedLib.Models.Common;

namespace UnitTests.Application.Musicians.Queries.GetMusicianAverageByPlayedInstrumentsType;

public sealed class GetMusicianByIdTests
{
    private readonly IMusicianRepository _musicianRepository;
    private readonly ILogger<GetMusicianByIdQueryHandler> _logger;
    private readonly GetMusicianByIdQuery _query;
    private readonly GetMusicianByIdQueryHandler _handler;


    public GetMusicianByIdTests()
    {
        _musicianRepository = Substitute.For<IMusicianRepository>();
        _logger = Substitute.For<ILogger<GetMusicianByIdQueryHandler>>();
        _query = new(1);
        _handler = new(_musicianRepository, _logger);
    }

    [Fact]
    public async Task Handler_Should_Return_Error_When_Request_Is_Null()
    {
        //Arrange        

        //Act
        var result = await _handler.Handle(null, CancellationToken.None);

        //Assert
        await _musicianRepository.Received(0).GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handler_Should_Return_Error_When_RequestId_Is_Zero()
    {
        //Arrange
        var query = new GetMusicianByIdQuery(0);

        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        await _musicianRepository.Received(0).GetByIdAsync(Arg.Any<int>(), CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handler_Should_Return_Success_When_RequestId_Greater_Than_Zero()
    {
        //Arrange

        //Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        //Assert
        await _musicianRepository.Received(1).GetByIdAsync(_query.Id, Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }
 
}

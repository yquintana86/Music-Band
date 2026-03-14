using Application.Abstractions.Repositories;
using Application.Musicians.Query.IternationalActivitiesByMusician;
using Application.Musicians.Query.SearchMusicianById;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SharedLib.Models.Common;

namespace UnitTests.Application.Musicians.Queries.IternationalActivitiesByMusician;

public sealed class IternationalActivitiesByMusicianTests
{
    private readonly IMusicianRepository _musicianRepository;
    private readonly IActivityRepository _activityRepository;
    private readonly ILogger<IternationalActivitiesByMusicianQueryHandler> _logger;
    private readonly InternationalActivitiesByMusicianQuery _query = new(2);
    private readonly IternationalActivitiesByMusicianQueryHandler _handler;


    public IternationalActivitiesByMusicianTests()
    {
        _musicianRepository = Substitute.For<IMusicianRepository>();
        _activityRepository = Substitute.For<IActivityRepository>();
        _logger = Substitute.For<ILogger<IternationalActivitiesByMusicianQueryHandler>>();
        _handler = new IternationalActivitiesByMusicianQueryHandler(_activityRepository, _musicianRepository, _logger);
    }

    [Fact]
    public async Task Handler_Should_Return_Error_When_Request_Is_Null()
    {
        //Arrange        

        //Act
        var result = await _handler.Handle(null, CancellationToken.None);

        //Assert
        await _activityRepository.Received(0).GetInternationalActivitiesByMusicianIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handler_Should_Return_Error_When_RequestId_Is_Zero()
    {
        //Arrange
        var query = new InternationalActivitiesByMusicianQuery(0);

        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        await _activityRepository.Received(0).GetInternationalActivitiesByMusicianIdAsync(Arg.Any<int>(), CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handler_Should_Return_Error_When_Musician_NotFound()
    {
        //Arrange
        _musicianRepository.GetByIdAsync(Arg.Any<int>()).Returns(Task.FromResult((Musician?)null));

        //Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        //Assert
        await _musicianRepository.Received(1).GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handler_Should_Success_When_Musician_Is_Found()
    {
        //Arrange 
        var musician = new Musician();
        _musicianRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(musician));

        //Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        //Assert
        await _musicianRepository.Received(1).GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
        await _activityRepository.Received(1).GetInternationalActivitiesByMusicianIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }


}

using Application.Abstractions.Repositories;
using Application.Musicians.Query.IternationalActivitiesByMusician;
using Application.Musicians.Query.SearchDomesticSeniorMusicians;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SharedLib.Models.Common;

namespace UnitTests.Application.Musicians.Queries.SearchDomesticSeniorMusicians;

public sealed class SearchDomesticSeniorMusiciansTests
{
    private readonly IMusicianRepository _musicianRepository;
    private readonly SearchDomesticSeniorMusiciansQuery _query = new(16);
    private readonly ILogger<SearchDomesticSeniorMusiciansQueryHandler> _logger;
    private readonly SearchDomesticSeniorMusiciansQueryHandler _handler;

    public SearchDomesticSeniorMusiciansTests()
    {
        _musicianRepository = Substitute.For<IMusicianRepository>();
        _logger = Substitute.For<ILogger<SearchDomesticSeniorMusiciansQueryHandler>>();
        _handler = new SearchDomesticSeniorMusiciansQueryHandler(_musicianRepository, _logger);
    }

    [Fact]
    public async Task Handler_Should_Return_Error_When_Request_Is_Null()
    {
        //Arrange        

        //Act
        var result = await _handler.Handle(null, CancellationToken.None);

        //Assert
        await _musicianRepository.Received(0).SearchNoInternationalMusicianOlderThanAge(Arg.Any<int>(), Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(14)]
    [InlineData(15)]
    public async Task Handler_Should_Return_Error_When_Age_Is_Less_Than_16(int age)
    {
        //Arrange
        var query = new SearchDomesticSeniorMusiciansQuery(age);

        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        await _musicianRepository.Received(0).SearchNoInternationalMusicianOlderThanAge(Arg.Any<int>(), CancellationToken.None);
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handler_Should_Return_Success_When_Age_Greater_Than_15()
    {
        //Arrange 

        //Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        //Assert
        await _musicianRepository.Received(1).SearchNoInternationalMusicianOlderThanAge(_query.age, Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }


}

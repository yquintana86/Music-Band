using Application.Abstractions.Repositories;
using Application.Instrument.Queries.SearchInstrumentbyId;
using Application.Instruments.Queries.GetMostUsedInstruments;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SharedLib.Models.Common;

namespace UnitTests.Application.Instruments.Queries.GetInstrumentById;

public sealed class GetInstrumentByIdTest
{
    private readonly IInstrumentRepository _instrumentRepository;
    private readonly ILogger<GetMusicalInstrumentByIdQueryHandler> _logger;
    private readonly GetMusicalInstrumentByIdQuery _query = new (1);
    private readonly GetMusicalInstrumentByIdQueryHandler _handler;


    public GetInstrumentByIdTest()
    {
        _instrumentRepository = Substitute.For<IInstrumentRepository>();
        _logger = Substitute.For<ILogger<GetMusicalInstrumentByIdQueryHandler>>();
        _handler = new GetMusicalInstrumentByIdQueryHandler(_instrumentRepository, _logger);
    }

    [Fact]
    public async Task Handler_Should_Return_Error_When_Request_Is_Null()
    {
        //Arrange 

        //Act
            var result =  await _handler.Handle(null, CancellationToken.None);

        //Assert
        await _instrumentRepository.Received(0).GetMostUsedInstrument(null, Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handler_Should_Return_Error_When_Id_Is_Zero()
    {
        //Arrange
        var request = new GetMusicalInstrumentByIdQuery(0);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        await _instrumentRepository.Received(0).GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handler_Should_Return_Success_When_QueryId_Is_Greater_Than_Zero()
    {
        //Arrange

        //Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        //Assert
        await _instrumentRepository.Received(1).GetByIdAsync(_query.id, Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }



}

using Application.Abstractions.Repositories;
using Application.Instruments.Queries.GetMostUsedInstruments;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SharedLib.Models.Common;

namespace UnitTests.Application.Instruments.Queries.GetMostUsedInstruments;

public sealed class GetMostUsedInstrumentsTests
{
    private readonly GetMostUsedInstrumentQuery _query = new(2);
    private readonly IInstrumentRepository _instrumentRepository;
    private readonly ILogger<GetMostUsedInstrumentQueryHandler> _logger;
    private readonly GetMostUsedInstrumentQueryHandler _handler;

    public GetMostUsedInstrumentsTests()
    {
        _instrumentRepository = Substitute.For<IInstrumentRepository>();
        _logger = Substitute.For<ILogger<GetMostUsedInstrumentQueryHandler>>();
        _handler = new GetMostUsedInstrumentQueryHandler(_instrumentRepository, _logger);
    }

    [Fact]
    public async Task Handler_Should_Return_Error_When_Request_Is_null()
    {
        //Arrange

        //Act
        var result = await _handler.Handle(null, CancellationToken.None);

        //Assert
        await _instrumentRepository.Received(0).GetMostUsedInstrument(Arg.Any<int>(),Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handler_Should_Return_Error_When_Request_Id_Zero()
    {
        //Arrange
        var query = new GetMostUsedInstrumentQuery(0);

        //Act
        var result = await _handler.Handle(query, CancellationToken.None);

        //Assert
        await _instrumentRepository.Received(0).GetMostUsedInstrument(query.InstrumentQtyToSearch,Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handler_Should_Return_Success_When_Id_Greater_Than_Zero()
    {
        //Arrange

        //Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        //Assert
        await _instrumentRepository.Received(1).GetMostUsedInstrument(_query.InstrumentQtyToSearch, Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }
    
}

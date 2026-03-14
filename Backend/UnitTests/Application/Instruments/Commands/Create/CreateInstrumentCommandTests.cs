using Application.Abstractions.DataContext;
using Application.Abstractions.Repositories;
using Application.Instrument.Commands.CreateInstrument;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UnitTests.Instruments.Commands;

public sealed class CreateInstrumentCommandTests
{
    private readonly CreateInstrumentCommand _command = new("Flaute", "Spain", null, Shared.Common.InstrumentType.Woodwind, 1);
    private readonly CreateInstrumentCommandHandler _hanlder;
    private readonly ILogger<CreateInstrumentCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IInstrumentRepository _instrumentRepository;
    private readonly IMusicianRepository _musicianRepository;

    public CreateInstrumentCommandTests()
    {
        _instrumentRepository = Substitute.For<IInstrumentRepository>();
        _musicianRepository = Substitute.For<IMusicianRepository>();
        _logger = Substitute.For<ILogger<CreateInstrumentCommandHandler>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _hanlder = new(_instrumentRepository, _musicianRepository, _logger, _unitOfWork);
    }


    [Fact]
    public async Task Hanlder_Should_ReturnError_WhenOwnerNotFound()
    {
        //Arrange
        _musicianRepository.GetByIdAsync(_command.MusicianId).Returns((Musician?)null);

        //Act
        var result = await _hanlder.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == SharedLib.Models.Common.ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_When_Musician_Exists()
    {
        //Arrange
        _musicianRepository.GetByIdAsync(_command.MusicianId).Returns(new Musician());

        //Act
        var result = await _hanlder.Handle(_command, default);

        //Assert
        _instrumentRepository.Received(1).Add(Arg.Any<MusicalInstrument>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNull();

    }


}

using Application.Abstractions.DataContext;
using Application.Abstractions.Repositories;
using Application.Instrument.Commands.UpdateInstrument;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SharedLib.Models.Common;
using System.Linq.Expressions;

namespace UnitTests.Application.Instruments.Commands.Update;

public sealed class UpdateInstrumentCommandTests
{
    private readonly UpdateInstrumentCommand _command = new(1,"Guitar", "Test", "test", Shared.Common.InstrumentType.StringInstrument, 1);
    private readonly UpdateInstrumentCommandHandler _handler;
    private readonly IInstrumentRepository _instrumentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMusicianRepository _musicianRepository;
    private readonly ILogger<UpdateInstrumentCommandHandler> _logger;

    public UpdateInstrumentCommandTests()
    {
       _instrumentRepository = Substitute.For<IInstrumentRepository>();
       _unitOfWork = Substitute.For<IUnitOfWork>();
       _musicianRepository = Substitute.For<IMusicianRepository>();
       _logger = Substitute.For<ILogger<UpdateInstrumentCommandHandler>>();
        _handler = new UpdateInstrumentCommandHandler(_instrumentRepository, _musicianRepository, _logger, _unitOfWork);
    }


    [Fact]
    public async Task Handler_Should_Return_Error_When_Musician_Not_Found()
    {
        //Arrange
        _musicianRepository.GetByIdWithRelatedEntitiesAsync(_command.MusicianId, 
            Arg.Any<Expression<Func<Musician, ICollection<MusicalInstrument>>>>(), 
            Arg.Any<CancellationToken>()).Returns(Task.FromResult((Musician?)null));

        //Act
        var result = await _handler.Handle(_command, CancellationToken.None);

        //Assert
        await _musicianRepository.Received(1).GetByIdWithRelatedEntitiesAsync(_command.MusicianId,
            Arg.Any<Expression<Func<Musician, ICollection<MusicalInstrument>>>>(),
            Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handler_Should_Return_Error_When_Instrument_Not_Found()
    {
        //Arrange
        var musician = new Musician
        {
            Id = 1,
            Instruments = new List<MusicalInstrument> {
                    new() {
                        Id = 2,
                        Name = "test",
                        MusicianId = 1,
                        Musician = new(),
                        Type = Shared.Common.InstrumentType.Percussion,
                        Country = "test",
                        Description = "test",
                    },
                    new() {
                        Id = 3,
                        Name = "Guitar",
                        MusicianId = 1,
                        Musician = new(),
                        Type = Shared.Common.InstrumentType.Percussion,
                        Country = "test",
                        Description = "test",
                    }
                }
        };
        _musicianRepository.GetByIdWithRelatedEntitiesAsync(_command.MusicianId,
            Arg.Any<Expression<Func<Musician, ICollection<MusicalInstrument>>>>(),
            Arg.Any<CancellationToken>()).Returns(Task.FromResult<Musician?>(musician));

        //Act
        var result = await _handler.Handle(_command, CancellationToken.None);


        //Assert
        await _musicianRepository.Received(1).GetByIdWithRelatedEntitiesAsync(_command.MusicianId,
            Arg.Any<Expression<Func<Musician, ICollection<MusicalInstrument>>>>(),
            Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handle_Should_Return_Error_When_Musician_Already_Owns_Instrument_Name()
    {
        //Arrange
        var musician = new Musician
        {
            Id = 1,
            Instruments = new List<MusicalInstrument> {
                    new() {
                        Id = 1,
                        Name = "test",
                        MusicianId = 1,
                        Musician = new(),
                        Type = Shared.Common.InstrumentType.Percussion,
                        Country = "test",
                        Description = "test",
                    },
                    new() {
                        Id = 3,
                        Name = "Guitar",
                        MusicianId = 1,
                        Musician = new(),
                        Type = Shared.Common.InstrumentType.Percussion,
                        Country = "test",
                        Description = "test",
                    }
                }
        };
        _musicianRepository.GetByIdWithRelatedEntitiesAsync(_command.MusicianId,
            Arg.Any<Expression<Func<Musician, ICollection<MusicalInstrument>>>>(),
            Arg.Any<CancellationToken>()).Returns(Task.FromResult<Musician?>(musician));

        //Act
        var result = await _handler.Handle(_command, CancellationToken.None);

        //Assert
        await _musicianRepository.Received(1).GetByIdWithRelatedEntitiesAsync(_command.MusicianId,
            Arg.Any<Expression<Func<Musician, ICollection<MusicalInstrument>>>>(),
            Arg.Any<CancellationToken>());
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handle_Should_Return_Success_When_Musician_DoesNot_Have_Any_Instrument_With_Update_Name()
    {
        //Arrange
        var musician = new Musician
        {
            Id = 1,
            Instruments = new List<MusicalInstrument> {
                    new() {
                        Id = 1,
                        Name = "test",
                        MusicianId = 1,
                        Musician = new(),
                        Type = Shared.Common.InstrumentType.Percussion,
                        Country = "test",
                        Description = "test",
                    },
                    new() {
                        Id = 3,
                        Name = "test",
                        MusicianId = 1,
                        Musician = new(),
                        Type = Shared.Common.InstrumentType.Percussion,
                        Country = "test",
                        Description = "test",
                    }
                }
        };
        _musicianRepository.GetByIdWithRelatedEntitiesAsync(_command.MusicianId,
            Arg.Any<Expression<Func<Musician, ICollection<MusicalInstrument>>>>(),
            Arg.Any<CancellationToken>()).Returns(Task.FromResult<Musician?>(musician));

        //Act

        var result = await _handler.Handle(_command, CancellationToken.None);

        //Assert
        await _musicianRepository.Received(1).GetByIdWithRelatedEntitiesAsync(
            _command.Id,
            Arg.Any<Expression<Func<Musician, ICollection<MusicalInstrument>>>>(),
            Arg.Any<CancellationToken>());

        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }



}

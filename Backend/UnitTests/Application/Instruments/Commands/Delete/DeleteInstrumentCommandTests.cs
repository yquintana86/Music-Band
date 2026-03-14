using Application.Abstractions.DataContext;
using Application.Abstractions.Repositories;
using Application.Instrument.Commands.DeleteInstrument;
using Domain.Entities;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SharedLib.Models.Common;
using System.Linq.Expressions;
using System.Threading;

namespace UnitTests.Application.Instruments.Commands.Delete;

public sealed class DeleteInstrumentCommandTests
{
    private readonly DeleteInstrumentCommand _deleteInstrumentCommand = new(1);
    private readonly DeleteInstrumentCommandHandler _handler;
    private readonly ILogger<DeleteInstrumentCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IInstrumentRepository _instrumentRepository;
    private readonly IMusicianRepository _musicianRepository;


    public DeleteInstrumentCommandTests()
    {
        _logger = Substitute.For<ILogger<DeleteInstrumentCommandHandler>>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _instrumentRepository = Substitute.For<IInstrumentRepository>();
        _musicianRepository = Substitute.For<IMusicianRepository>();
        _handler = new DeleteInstrumentCommandHandler(_instrumentRepository, _logger, _musicianRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenInstrumentNotFound()
    {
        //Arrange
        _instrumentRepository.GetByIdAsync(_deleteInstrumentCommand.Id).Returns(Task.FromResult((MusicalInstrument?)null));

        //Act
        var result = await _handler.Handle(_deleteInstrumentCommand, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenOwnerNotFound()
    {
        //Arrange 
        _musicianRepository.GetByIdWithRelatedEntitiesAsync(
            1,
            Arg.Any<Expression<Func<Musician, ICollection<MusicalInstrument>>>>(),
            Arg.Any<CancellationToken>())
            .Returns(Task.FromResult((Musician?)null));

        //Act
        var result = await _handler.Handle(_deleteInstrumentCommand, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handler_Should_ReturnError_WhenMusicianInstrumentsQtyHasLastOne()
    {
        //Arrange

        var musicianDb = new Musician
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
                    }}
        };

        _musicianRepository.GetByIdWithRelatedEntitiesAsync(
            musicianDb.Id,
            Arg.Any<Expression<Func<Musician, ICollection<MusicalInstrument>>>>(),
            Arg.Any<CancellationToken>()).Returns(
            Task.FromResult<Musician?>(musicianDb));

        //Act
        var result = await _handler.Handle(_deleteInstrumentCommand, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenMusicianInstrumentsQtyHasMoreThanOne()
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
                        Id = 2,
                        Name = "test2",
                        MusicianId = 1,
                        Musician = new(),
                        Type = Shared.Common.InstrumentType.Percussion,
                        Country = "test",
                        Description = "test",
                    }
                }
        };

        _instrumentRepository.GetByIdAsync(_deleteInstrumentCommand.Id).Returns(Task.FromResult<MusicalInstrument?>(musician.Instruments.First()));

        _musicianRepository.GetByIdWithRelatedEntitiesAsync(Arg.Any<int>(),
            Arg.Any<Expression<Func<Musician, ICollection<MusicalInstrument>>>>(),
            Arg.Any<CancellationToken>()).Returns(
            Task.FromResult<Musician?>(musician));

        //Act
        var result = await _handler.Handle(_deleteInstrumentCommand, default);

        //Assert
        await _instrumentRepository.Received(1).GetByIdAsync(musician.Instruments.First().Id, Arg.Any<CancellationToken>());
        await _musicianRepository.Received(1).GetByIdWithRelatedEntitiesAsync(
            musician.Id,
            Arg.Any<Expression<Func<Musician, ICollection<MusicalInstrument>>>>(),
            Arg.Any<CancellationToken>());

        await _unitOfWork.Received(1).SaveChangesAsync(CancellationToken.None);
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }


}

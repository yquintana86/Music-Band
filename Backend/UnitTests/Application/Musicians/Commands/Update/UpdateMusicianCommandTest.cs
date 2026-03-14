using Application.Abstractions.DataContext;
using Application.Abstractions.Repositories;
using Application.Musicians.Command.UpdateMusician;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SharedLib.Models.Common;

namespace UnitTests.Application.Musicians.Commands.Update;

public sealed class UpdateMusicianCommandTest
{
    private readonly IMusicianRepository _musicianRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateMusicianCommandHandler> _logger;
    private readonly UpdateMusicianCommand _command = new UpdateMusicianCommand(1, "", null, "", 18, 10, 75);
    private readonly UpdateMusicianCommandHandler _handler;


    public UpdateMusicianCommandTest()
    {
        _musicianRepository = Substitute.For<IMusicianRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _logger = Substitute.For<ILogger<UpdateMusicianCommandHandler>>();
        _handler = new(_musicianRepository, _logger, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenMusicianIdNotFound()
    {
        //Arrange
        _musicianRepository
        .GetByIdAsync(_command.Id)
        .Returns((Musician?)null);

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenMusicianIdIsFound()
    {
        //Arrange
        _musicianRepository.GetByIdAsync(_command.Id).Returns(new Musician());

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNull();
    }
}

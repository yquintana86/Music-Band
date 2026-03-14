using Application.Abstractions.DataContext;
using Application.Abstractions.Repositories;
using Application.Musicians.Command.DeleteMusician;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UnitTests.Application.Musicians.Commands.Delete;

public sealed class DeleteMusicianCommandTests
{
    private readonly DeleteMusicianCommandHandler _handler;
    private readonly DeleteMusicianCommand _command = new(1);
    private readonly IMusicianRepository _musicianRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteMusicianCommandHandler> _logger;

    public DeleteMusicianCommandTests()
    {
        _musicianRepository = Substitute.For<IMusicianRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _logger = Substitute.For<ILogger<DeleteMusicianCommandHandler>>();
        _handler = new (_musicianRepository,_logger, _unitOfWork);
    }

    [Fact]
    public async Task Handler_Should_ReturnError_WhenMusicianIdNotFound()
    {
        //Arrange
        _musicianRepository.ExistIdAsync(Arg.Is<int>(id => _command.Id == id)).Returns(false);

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == SharedLib.Models.Common.ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handler_Should_ReturnSuccess_WhenMusicianIdIsFound()
    {
        //Arrange
        _musicianRepository.ExistIdAsync(Arg.Is<int>(id => id == _command.Id), default).Returns(true);

        //Act
        var result = await _handler.Handle(_command, default);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNull();
    }


}

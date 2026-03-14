using Application.Abstractions.DataContext;
using Application.Abstractions.Repositories;
using Application.PaymentDetails.Commands.CreatePaymentDetails;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.Core;
using SharedLib.Models.Common;
using System.Net.WebSockets;
using System.Threading;

namespace UnitTests.Application.PaymentDetails.Commands;

public sealed class CreatePaymentDetailsTests
{
    private readonly IMusicianPaymentDetailsRepository _musicianPaymentDetailsRepository;
    private readonly IMusicianRepository _musicianRepository;
    private readonly IRangePlusRepository _rangePlusRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreatePaymentDetailCommandHandler> _logger;
    private readonly CreatePaymentDetailCommandHandler _handler;    

    public CreatePaymentDetailsTests()
    {
        _musicianPaymentDetailsRepository = Substitute.For<IMusicianPaymentDetailsRepository>();
        _musicianRepository = Substitute.For<IMusicianRepository>();
        _rangePlusRepository = Substitute.For<IRangePlusRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _logger = Substitute.For<ILogger<CreatePaymentDetailCommandHandler>>();
        _handler = new CreatePaymentDetailCommandHandler(_musicianPaymentDetailsRepository, _unitOfWork, _logger, _rangePlusRepository, _musicianRepository); 
    }

    private static CreatePaymentDetailCommand CreateCommand(
        DateTime? paymentDate,
        decimal salary = 200,
        decimal basicSalary = 50,
        int musicianId = 1,
        int rangePlusId = 1)
    {
        return new CreatePaymentDetailCommand(
            paymentDate ?? new DateTime(2025, 1, 1),
            salary,
            basicSalary,
            musicianId,
            rangePlusId);
    }

    [Fact]
    public async Task Hanlder_Should_Return_Error_When_Musician_NotFound()
    {
        //Arrange
        _musicianRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult<Musician?>(null));
        var command = CreateCommand(paymentDate: DateTime.Now);
        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        await _musicianRepository.Received(1).GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
        await _rangePlusRepository.Received(0).GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
        _musicianPaymentDetailsRepository.Received(0).Add(Arg.Any<MusicianPaymentDetail>());
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handler_Should_Return_Error_When_RangePlus_NotFound()
    {
        //Arrange
        var command = CreateCommand(paymentDate: DateTime.Now);
        _musicianRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult<Musician?>(new()));
        _rangePlusRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult<RangePlus?>(null));

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        await _musicianRepository.Received(1).GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
        await _rangePlusRepository.Received(1).GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
        _musicianPaymentDetailsRepository.Received(0).Add(Arg.Any<MusicianPaymentDetail>());
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorType == ApiErrorType.Validation);
    }

    [Fact]
    public async Task Handler_Should_Return_Success_When_RangePlus_Exists()
    {
        //Arrange
        var command = CreateCommand(DateTime.Now);
        _musicianRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult<Musician?>(new()));
        _rangePlusRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult<RangePlus?>(new()));

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        await _musicianRepository.Received(1).GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
        await _rangePlusRepository.Received(1).GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>());
        _musicianPaymentDetailsRepository.Received(1).Add(Arg.Any<MusicianPaymentDetail>());
        await _unitOfWork.Received(1).SaveChangesAsync();
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
    }

}

using Microsoft.AspNetCore.Mvc;
using Moq;
using MoqExercise.Controllers;
using MoqExercise.Models;
using MoqExercise.Repositories;

namespace MoqExercise.Tests;

public class ReservationRepositoryTest
{
    private static readonly Reservation[] _reservations = [
        new() {
            Id = 1,
            Name = "Test One",
            StartLocation = "SL1",
            EndLocation = "EL1"
        },
        new() {
            Id = 2,
            Name = "Test Two",
            StartLocation = "SL2",
            EndLocation = "EL2"
        },
        new() {
            Id = 3,
            Name = "Test Three",
            StartLocation = "SL3",
            EndLocation = "EL3"
        }
    ];
    private static readonly int _numReservations = _reservations.Length;

    private static Reservation? GetSingleReservation(int id) =>
        _reservations.FirstOrDefault(r => r.Id == id);

    private Mock<IReservationRepository>? _mockRepo;
    private ReservationsController? _controller;

    private void SetupRepo(Action<Mock<IReservationRepository>> setupAction)
    {
        _mockRepo = new();
        setupAction(_mockRepo);
        _controller = new(_mockRepo.Object);
    }

    private static TValue? UnpackResultWithReturn<TResult, TValue>(ActionResult<TValue>? controllerResult) where TResult : ObjectResult
    {
        var actionResult1 = Assert.IsType<ActionResult<TValue>>(controllerResult);
        var actionResult2 = Assert.IsType<TResult>(actionResult1.Result);
        var value = (TValue?)actionResult2.Value;
        return value;
    }

    private static void UnpackResultWithoutReturn<TResult, TValue>(ActionResult<TValue>? controllerResult) where TResult : ObjectResult
    {
        var actionResult1 = Assert.IsType<ActionResult<TValue>>(controllerResult);
        Assert.IsType<TResult>(actionResult1.Result);
    }

    [Fact]
    public void Get_NoArguments_ReturnsAllReservations()
    {
        SetupRepo(mockRepo => mockRepo
            .Setup(repo => repo.Reservations)
            .Returns(_reservations));

        var controllerResult = _controller?.Get();

        var reservations = UnpackResultWithReturn<OkObjectResult, IEnumerable<Reservation>>(controllerResult);
        Assert.Equal(_numReservations, reservations?.Count());
    }

    private static IEnumerable<Reservation> Multiple() =>
        [
            new Reservation()
            {
                Id = 1,
                Name = "Test One",
                StartLocation = "SL1",
                EndLocation = "EL1"
            },
            new Reservation()
            {
                Id = 2,
                Name = "Test Two",
                StartLocation = "SL2",
                EndLocation = "EL2"
            },
            new Reservation()
            {
                Id = 3,
                Name = "Test Three",
                StartLocation = "SL3",
                EndLocation = "EL3"
            }
        ];

    [Fact]
    public void Get_ReservationWithId0_ReturnsBadRequest()
    {
        SetupRepo(mockRepo => mockRepo
            .Setup(repo => repo[It.IsAny<int>()])
            .Returns<int>((id) => GetSingleReservation(id)));

        int id = 0;
        var controllerResult = _controller?.Get(id);

        UnpackResultWithoutReturn<BadRequestObjectResult, Reservation>(controllerResult);

        //var actionResult1 = Assert.IsType<ActionResult<Reservation>>(controllerResult);
        //var actionResult2 = Assert.IsType<BadRequestObjectResult>(actionResult1.Result);
    }

    private static Reservation? Single(int id)
    {
        var reservations = Multiple();
        return reservations.Where(r => r.Id == id).FirstOrDefault();
    }

    [Fact]
    public void Get_ExistingReservation_ReturnsReservationOK()
    {
        SetupRepo(mockRepo => mockRepo
            .Setup(repo => repo[It.IsAny<int>()])
            .Returns<int>((id) => GetSingleReservation(id)));

        int id = 1;
        var controllerResult = _controller?.Get(id);

        var actionResult1 = Assert.IsType<ActionResult<Reservation>>(controllerResult);
        var actionResult2 = Assert.IsType<OkObjectResult>(actionResult1.Result);
        var reservation = (Reservation?)actionResult2.Value;
        Assert.Equal(id, reservation?.Id);
    }

    [Fact]
    public void Get_MissingReservation_ReturnsNotFound()
    {
        int id = 4;
        var mockRepo = new Mock<IReservationRepository>();
        mockRepo.Setup(repo => repo[It.IsAny<int>()]).Returns<int>((id) => Single(id));
        var controller = new ReservationsController(mockRepo.Object);

        var result = controller.Get(id);

        var actionResult = Assert.IsType<ActionResult<Reservation>>(result);
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public void Post_ValidReservation_AddsReservation()
    {
        Reservation r = new()
        {
            Id = 4,
            Name = "Test Four",
            StartLocation = "SL4",
            EndLocation = "EL4"
        };
        var mockRepo = new Mock<IReservationRepository>();
        mockRepo.Setup(repo => repo.AddReservation(It.IsAny<Reservation>())).Returns(r);
        var controller = new ReservationsController(mockRepo.Object);

        var result = controller.Post(r);

        var actionResult = Assert.IsType<ActionResult<Reservation>>(result);
        var actionValue = Assert.IsType<OkObjectResult>(actionResult.Result);
        var reservation = (Reservation)actionValue.Value;
        Assert.Equal(r.Id, reservation.Id);
        Assert.Equal(r.Name, reservation.Name);
        Assert.Equal(r.StartLocation, reservation.StartLocation);
        Assert.Equal(r.EndLocation, reservation.EndLocation);
    }

    [Fact]
    public void Put_ExistingReservation_UpdatesReservation()
    {
        Reservation r = new()
        {
            Id = 3,
            Name = "New Test Three",
            StartLocation = "New SL3",
            EndLocation = "New EL3"
        };
        var mockRepo = new Mock<IReservationRepository>();
        mockRepo.Setup(repo => repo.AddReservation(It.IsAny<Reservation>())).Returns(r);
        var controller = new ReservationsController(mockRepo.Object);

        var result = controller.Put(r);

        var actionResult = Assert.IsType<ActionResult<Reservation>>(result);
        var actionValue = Assert.IsType<OkObjectResult>(actionResult.Result);
        var reservation = (Reservation)actionValue.Value;
        Assert.Equal(r.Id, reservation.Id);
        Assert.Equal(r.Name, reservation.Name);
        Assert.Equal(r.StartLocation, reservation.StartLocation);
        Assert.Equal(r.EndLocation, reservation.EndLocation);
    }

    [Fact]
    public void Delete_ExistingReservation_RemovesReservation()
    {
        var mockRepo = new Mock<IReservationRepository>();
        mockRepo.Setup(repo => repo.DeleteReservation(It.IsAny<int>())).Verifiable();
        var controller = new ReservationsController(mockRepo.Object);

        controller.Delete(3);

        mockRepo.Verify();
    }
}
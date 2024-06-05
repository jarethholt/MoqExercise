using Microsoft.AspNetCore.Mvc;
using Moq;
using MoqExercise.Controllers;
using MoqExercise.Models;
using MoqExercise.Repositories;

namespace MoqExercise.Tests;

public class ReservationRepositoryTest
{
    [Fact]
    public void Get_AllReservations()
    {
        var mockRepo = new Mock<IReservationRepository>();
        mockRepo.Setup(repo => repo.Reservations).Returns(Multiple());
        var controller = new ReservationsController(mockRepo.Object);

        var result = controller.Get();

        var model = Assert.IsAssignableFrom<IEnumerable<Reservation>>(result);
        Assert.Equal(3, model.Count());
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
    public void Get_NonExistantReservation_ReturnsBadRequest()
    {
        int id = 0;
        var mockRepo = new Mock<IReservationRepository>();
        mockRepo.Setup(repo => repo[It.IsAny<int>()]).Returns<int>((a) => Single(a));
        var controller = new ReservationsController(mockRepo.Object);

        var result = controller.Get(id);

        var actionResult = Assert.IsType<ActionResult<Reservation>>(result);
        Assert.IsType<BadRequestObjectResult>(actionResult.Result);
    }

    public Reservation? Single(int id)
    {
        var reservations = Multiple();
        return reservations.Where(r => r.Id == id).FirstOrDefault();
    }
}
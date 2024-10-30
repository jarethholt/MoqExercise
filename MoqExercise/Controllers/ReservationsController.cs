using Microsoft.AspNetCore.Mvc;
using MoqExercise.Models;
using MoqExercise.Repositories;

namespace MoqExercise.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationsController(IReservationRepository repo) : ControllerBase
{
    private readonly IReservationRepository _repo = repo;

    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> Get() =>
        Ok(_repo.Reservations);

    [HttpGet("{id}")]
    public ActionResult<Reservation> Get(int id)
    {
        if (id == 0)
            return BadRequest("Value must be passed in the request body");

        var reservation = _repo[id];
        if (reservation is null)
            return NotFound();
        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult<Reservation> Post([FromBody] Reservation res) =>
        CreatedAtAction(nameof(Post), _repo.AddReservation(new Reservation
        {
            Name = res.Name,
            StartLocation = res.StartLocation,
            EndLocation = res.EndLocation
        }));

    [HttpPut]
    public ActionResult<Reservation> Put([FromBody] Reservation res) =>
        Ok(_repo.UpdateReservation(res));

    [HttpDelete("{id}")]
    public void Delete(int id) =>
        _repo.DeleteReservation(id);
}

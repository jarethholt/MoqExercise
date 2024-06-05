using MoqExercise.Models;

namespace MoqExercise.Repositories;

public interface IReservationRepository
{
    IEnumerable<Reservation> Reservations { get; }
    Reservation? this[int id] { get; }
    Reservation AddReservation(Reservation reservation);
    Reservation UpdateReservation(Reservation reservation);
    void DeleteReservation(int id);
}

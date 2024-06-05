using MoqExercise.Models;

namespace MoqExercise.Repositories;

public class ReservationRepository : IReservationRepository
{
    private Dictionary<int, Reservation> items = [];

    public ReservationRepository()
    {
        new List<Reservation>()
        {
            new() { Id = 1, Name = "Ankit", StartLocation = "New York", EndLocation = "Beijing" },
            new() { Id = 2, Name = "Bobby", StartLocation = "New Jersey", EndLocation = "Boston" },
            new() { Id = 3, Name = "Clara", StartLocation = "London", EndLocation = "Paris" },
        }.ForEach(r => AddReservation(r));
    }

    public Reservation? this[int id] =>
        items.TryGetValue(id, out Reservation? value) ? value : null;

    public IEnumerable<Reservation> Reservations =>
        items.Values;

    public Reservation AddReservation(Reservation reservation)
    {
        if (reservation.Id < 1)
        {
            int key = 1;
            while (items.ContainsKey(key)) key++;
            reservation.Id = key;
        }
        items[reservation.Id] = reservation;
        return reservation;
    }

    public void DeleteReservation(int id) =>
        items.Remove(id);

    public Reservation UpdateReservation(Reservation reservation) =>
        AddReservation(reservation);
}

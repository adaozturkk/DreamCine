using DreamCine.Application.DTOs.Reservation;
using DreamCine.Core.Models;

namespace DreamCine.Application.Mappers
{
    public static class ReservationMappers
    {
        public static ReservationDto ToReservationDto(this Reservation reservation, MovieSession session, List<Seat> bookedSeats)
        {
            return new ReservationDto
            {
                Id = reservation.Id,
                MovieSessionId = reservation.MovieSessionId,
                BookingTime = reservation.BookingTime,
                TotalPrice = reservation.TotalPrice,
                Status = reservation.Status.ToString(),
                Tickets = reservation.Tickets.Select(t => new ReservationTicketDto
                {
                    Id = t.Id,
                    MovieTitle = session.Movie?.Title,
                    SessionTime = session.SessionTime,
                    ScreenNumber = session.Screen?.ScreenNumber ?? 0,
                    SeatNumber = bookedSeats.FirstOrDefault(s => s.Id == t.SeatId)?.SeatNumber,
                    PurchasePrice = t.PurchasePrice,
                    Status = t.Status.ToString()
                }).ToList()
            };
        }
    }
}

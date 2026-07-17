using DreamCine.Application.DTOs.Ticket;
using DreamCine.Core.Models;

namespace DreamCine.Application.Mappers
{
    public static class TicketMappers
    {
        public static TicketDto ToTicketDto(this Ticket ticketModel)
        {
            return new TicketDto
            {
                Id = ticketModel.Id,
                UserEmail = ticketModel.Reservation?.User?.Email,
                MovieSessionId = ticketModel.MovieSessionId,
                MovieTitle = ticketModel.Reservation?.MovieSession?.Movie?.Title,
                SessionTime = ticketModel.Reservation?.MovieSession?.SessionTime ?? DateTime.MinValue,
                SeatId = ticketModel.SeatId,
                ScreenNumber = ticketModel.Reservation?.MovieSession?.Screen?.ScreenNumber ?? 0,
                ReservationId = ticketModel.ReservationId,
                SeatNumber = ticketModel.Seat?.SeatNumber ?? ticketModel.SeatId.ToString(),
                BookingTime = (DateTime)(ticketModel.Reservation?.BookingTime),
                PurchasePrice = ticketModel.PurchasePrice,
                Status = ticketModel.Status.ToString()
            };
        }
    }
}

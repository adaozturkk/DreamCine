using DreamCine.Application.DTOs.Ticket;
using DreamCine.Core.Models;
using DreamCine.Core.Enums;

namespace DreamCine.Application.Mappers
{
    public static class TicketMappers
    {
        public static Ticket ToTicketFromCreateDto(this CreateTicketDto createDto, string userId, decimal currentPrice)
        {
            return new Ticket
            {
                MovieSessionId = createDto.MovieSessionId,
                SeatId = createDto.SeatId,
                UserId = userId,
                PurchasePrice = currentPrice,
                BookingTime = DateTime.UtcNow,
                Status = TicketStatus.Pending
            };
        }

        public static TicketDto ToTicketDto(this Ticket ticketModel)
        {
            return new TicketDto
            {
                Id = ticketModel.Id,
                UserEmail = ticketModel.User?.Email,
                MovieSessionId = ticketModel.MovieSessionId,
                MovieTitle = ticketModel.MovieSession?.Movie?.Title,
                SessionTime = ticketModel.MovieSession?.SessionTime ?? DateTime.MinValue,
                SeatId = ticketModel.SeatId,
                ScreenNumber = ticketModel.MovieSession?.Screen?.ScreenNumber ?? 0,
                SeatNumber = ticketModel.Seat?.SeatNumber ?? ticketModel.SeatId.ToString(),
                BookingTime = ticketModel.BookingTime,
                PurchasePrice = ticketModel.PurchasePrice,
                Status = ticketModel.Status.ToString()
            };
        }
    }
}

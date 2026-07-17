using DreamCine.Application.Common;
using DreamCine.Application.DTOs.Ticket;
using DreamCine.Application.Interfaces;
using DreamCine.Application.Mappers;
using DreamCine.Core.Helpers;
using DreamCine.Core.Interfaces;

namespace DreamCine.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepo;

        public TicketService(ITicketRepository ticketRepo)
        {
            _ticketRepo = ticketRepo;
        }

        public async Task<ServiceResult<List<TicketDto>>> GetAllAsync(TicketQuery query)
        {
            var tickets = await _ticketRepo.GetAllAsync(query);
            var ticketDtos = tickets.Select(t => t.ToTicketDto()).ToList();

            return ServiceResult<List<TicketDto>>.Success(ticketDtos, 200);
        }

        public async Task<ServiceResult<TicketDto>> GetByIdAsync(int id, string userId, bool isAdmin)
        {
            var ticket = await _ticketRepo.GetByIdAsync(id);
            if (ticket == null)
            {
                return ServiceResult<TicketDto>.Failure("Ticket not found.", 404);
            }

            if (isAdmin || ticket.Reservation.UserId == userId)
            {
                var ticketDto = ticket.ToTicketDto();
                return ServiceResult<TicketDto>.Success(ticketDto, 200);
            }

            return ServiceResult<TicketDto>.Failure("Ticket not found.", 404);
        }

        public async Task<ServiceResult<List<TicketDto>>> GetUserTicketsAsync(string userId, UserTicketQuery query)
        {
            var tickets = await _ticketRepo.GetTicketsByUserIdAsync(userId, query);
            var ticketDtos = tickets.Select(t => t.ToTicketDto()).ToList();

            return ServiceResult<List<TicketDto>>.Success(ticketDtos, 200);
        }
    }
}

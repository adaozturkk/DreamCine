using DreamCine.Application.Common;
using DreamCine.Application.DTOs.Ticket;
using DreamCine.Core.Helpers;

namespace DreamCine.Application.Interfaces
{
    public interface ITicketService
    {
        Task<ServiceResult<List<TicketDto>>> GetUserTicketsAsync(string userId, UserTicketQuery query);
        Task<ServiceResult<List<TicketDto>>> GetAllAsync(TicketQuery query);
        Task<ServiceResult<TicketDto>> GetByIdAsync(int id, string userId, bool isAdmin);
    }
}

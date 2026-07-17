using DreamCine.Application.Common;
using DreamCine.Application.DTOs.Reservation;
using DreamCine.Core.Helpers;

namespace DreamCine.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ServiceResult<ReservationDto>> CreateReservationAsync(CreateReservationDto dto, string userId);
        Task<ServiceResult<List<ReservationDto>>> GetReservationsByUserIdAsync(string userId, UserReservationQueryObject query);
        Task<ServiceResult<List<ReservationDto>>> GetAllWithFilteringAsync(ReservationQueryObject query);
        Task<ServiceResult<ReservationDto>> GetByIdAsync(int id, string userId, bool isAdmin);
        Task<ServiceResult<string>> CancelReservationAsync(int id, string userId, bool isAdmin);
    }
}

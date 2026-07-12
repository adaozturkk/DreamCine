using DreamCine.Application.Common;
using DreamCine.Application.DTOs.Reservation;

namespace DreamCine.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ServiceResult<ReservationDto>> CreateReservationAsync(CreateReservationDto dto, string userId);
    }
}

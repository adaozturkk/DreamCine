using DreamCine.Application.Interfaces;
using DreamCine.Core.Interfaces;

namespace DreamCine.Infrastructure.Services
{
    public class ReservationJobService : IReservationJobService
    {
        private readonly IReservationRepository _reservationRepo;

        public ReservationJobService(IReservationRepository reservationRepo)
        {
            _reservationRepo = reservationRepo;
        }

        public async Task CancelUnpaidReservationAsync(int reservationId)
        {
            var reservation = await _reservationRepo.GetByIdAsync(reservationId);
            if (reservation == null || reservation.Status != Core.Enums.ReservationStatus.Pending)
            {
                return;
            }

            reservation.Status = Core.Enums.ReservationStatus.Cancelled;
            foreach (var ticket in reservation.Tickets)
            {
                ticket.Status = Core.Enums.TicketStatus.Cancelled;
            }

            await _reservationRepo.UpdateAsync(reservationId, reservation);
        }
    }
}

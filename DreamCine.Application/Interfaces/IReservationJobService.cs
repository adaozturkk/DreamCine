namespace DreamCine.Application.Interfaces
{
    public interface IReservationJobService
    {
        Task CancelUnpaidReservationAsync(int reservationId);
    }
}

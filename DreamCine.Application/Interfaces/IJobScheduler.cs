namespace DreamCine.Application.Interfaces
{
    public interface IJobScheduler
    {
        void ScheduleCancelReservationJob(int reservationId, TimeSpan delay);
    }
}

using DreamCine.Application.Interfaces;
using Hangfire;

namespace DreamCine.Infrastructure.Services
{
    public class HangfireJobScheduler : IJobScheduler
    {
        private readonly IBackgroundJobClient _jobClient;

        public HangfireJobScheduler(IBackgroundJobClient jobClient)
        {
            _jobClient = jobClient;
        }

        public void ScheduleCancelReservationJob(int reservationId, TimeSpan delay)
        {
            _jobClient.Schedule<IReservationJobService>(
                job => job.CancelUnpaidReservationAsync(reservationId), delay
            );
        }
    }
}

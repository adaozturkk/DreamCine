using DreamCine.Application.Common;
using DreamCine.Application.DTOs.Reservation;
using DreamCine.Application.Interfaces;
using DreamCine.Application.Mappers;
using DreamCine.Core.Helpers;
using DreamCine.Core.Interfaces;
using DreamCine.Core.Models;

namespace DreamCine.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepo;
        private readonly IMovieSessionRepository _movieSessionRepo;
        private readonly ITicketRepository _ticketRepo;
        private readonly ISeatRepository _seatRepo;
        private readonly IJobScheduler _jobScheduler;

        public ReservationService(IReservationRepository reservationRepo, IMovieSessionRepository movieSessionRepo,
            ITicketRepository ticketRepo, ISeatRepository seatRepo, IJobScheduler jobScheduler)
        {
            _reservationRepo = reservationRepo;
            _movieSessionRepo = movieSessionRepo;
            _ticketRepo = ticketRepo;
            _seatRepo = seatRepo;
            _jobScheduler = jobScheduler;
        }

        public async Task<ServiceResult<string>> CancelReservationAsync(int id, string userId, bool isAdmin)
        {
            var reservation = await _reservationRepo.GetByIdAsync(id);
            if (reservation == null)
            {
                return ServiceResult<string>.Failure("Reservation not found.", 404);
            }

            if (!isAdmin && reservation.UserId != userId)
            {
                return ServiceResult<string>.Failure("Reservation not found.", 404);
            }

            if (reservation.Status != Core.Enums.ReservationStatus.Pending)
            {
                return ServiceResult<string>.Failure("Only pending reservations can be cancelled.", 400);
            }

            reservation.Status = Core.Enums.ReservationStatus.Cancelled;
            foreach (var ticket in reservation.Tickets)
            {
                ticket.Status = Core.Enums.TicketStatus.Cancelled;
            }
            await _reservationRepo.UpdateAsync(id, reservation);

            return ServiceResult<string>.Success("Reservation cancelled successfully.", 200);
        }

        public async Task<ServiceResult<ReservationDto>> CreateReservationAsync(CreateReservationDto dto, string userId)
        {
            var session = await _movieSessionRepo.GetByIdAsync(dto.MovieSessionId);
            if (session == null)
            {
                return ServiceResult<ReservationDto>.Failure("Selected movie session does not exist.", 404);
            }

            var bookedSeats = await _seatRepo.GetSeatsByIdAsync(dto.SeatIds);
            if (bookedSeats == null || dto.SeatIds.Count != bookedSeats.Count)
            {
                return ServiceResult<ReservationDto>.Failure("One or more selected seats do not exist.", 400);
            }

            var occupiedSeats = await _ticketRepo.GetOccupiedSeatIdsAsync(session.Id);
            if (dto.SeatIds.Any(seatId => occupiedSeats.Contains(seatId)))
            {
                return ServiceResult<ReservationDto>.Failure("One or more selected seats are already reserved.", 400);
            }

            var reservation = new Reservation
            {
                UserId = userId,
                MovieSessionId = session.Id,
                BookingTime = DateTime.UtcNow,
                Status = Core.Enums.ReservationStatus.Pending,
                Tickets = new List<Ticket>()
            };

            foreach (var seatId in dto.SeatIds)
            {
                reservation.Tickets.Add(new Ticket
                {
                    SeatId = seatId,
                    PurchasePrice = session.Price,
                    Status = Core.Enums.TicketStatus.Pending,
                    MovieSessionId = session.Id
                });
            }

            reservation.TotalPrice = reservation.Tickets.Sum(x => x.PurchasePrice);
            await _reservationRepo.CreateAsync(reservation);

            _jobScheduler.ScheduleCancelReservationJob(reservation.Id, TimeSpan.FromMinutes(10));

            var reservationDto = reservation.ToReservationDto(session, bookedSeats);
            return ServiceResult<ReservationDto>.Success(reservationDto, 200);
        }

        public async Task<ServiceResult<List<ReservationDto>>> GetAllWithFilteringAsync(ReservationQueryObject query)
        {
            var reservation = await _reservationRepo.GetAllWithFilteringAsync(query);
            var reservationDtos = reservation.Select(r => r.ToReservationDto(
                r.MovieSession,
                r.Tickets.Select(t => t.Seat).ToList())
            ).ToList();

            return ServiceResult<List<ReservationDto>>.Success(reservationDtos, 200);
        }

        public async Task<ServiceResult<ReservationDto>> GetByIdAsync(int id, string userId, bool isAdmin)
        {
            var reservation = await _reservationRepo.GetByIdAsync(id);
            if (reservation == null)
            {
                return ServiceResult<ReservationDto>.Failure("Reservation not found.", 404);
            }

            if (isAdmin || reservation.UserId == userId)
            {
                var reservationDto = reservation.ToReservationDto(reservation.MovieSession, reservation.Tickets.Select(t => t.Seat).ToList());
                return ServiceResult<ReservationDto>.Success(reservationDto, 200);
            }

            return ServiceResult<ReservationDto>.Failure("Reservation not found.", 404);
        }

        public async Task<ServiceResult<List<ReservationDto>>> GetReservationsByUserIdAsync(string userId, UserReservationQueryObject query)
        {
            var reservations = await _reservationRepo.GetReservationsByUserIdAsync(userId, query);
            var reservationDtos = reservations.Select(r => r.ToReservationDto(
                r.MovieSession,
                r.Tickets.Select(t => t.Seat).ToList())
            ).ToList();

            return ServiceResult<List<ReservationDto>>.Success(reservationDtos, 200);
        }
    }
}

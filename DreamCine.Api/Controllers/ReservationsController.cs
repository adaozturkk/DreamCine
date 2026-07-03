using DreamCine.Application.DTOs.Reservation;
using DreamCine.Application.Interfaces;
using DreamCine.Application.Mappers;
using DreamCine.Core.Interfaces;
using DreamCine.Core.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DreamCine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        public IReservationRepository _reservationRepo;
        public IMovieSessionRepository _sessionRepo;
        public ITicketRepository _ticketRepo;
        public ISeatRepository _seatRepo;
        public IBackgroundJobClient _backgroundJobClient;

        public ReservationsController(IMovieSessionRepository sessionRepo, ITicketRepository ticketRepo, 
            IReservationRepository reservationRepo, ISeatRepository seatRepo, IBackgroundJobClient backgroundJobClient)
        {
            _sessionRepo = sessionRepo;
            _ticketRepo = ticketRepo;
            _reservationRepo = reservationRepo;
            _seatRepo = seatRepo;
            _backgroundJobClient = backgroundJobClient;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto reservationDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID could not be found in the token.");
            }

            var session = await _sessionRepo.GetByIdAsync(reservationDto.MovieSessionId);
            if (session == null)
            {
                return NotFound("Session id not found.");
            }

            var occupiedSeats = await _ticketRepo.GetOccupiedSeatIdsAsync(session.Id);
            if (reservationDto.SeatIds.Any(seatId => occupiedSeats.Contains(seatId)))
            {
                return BadRequest("Selected seats have already reserved.");
            }

            var reservation = new Reservation
            {
                UserId = userId,
                MovieSessionId = session.Id,
                BookingTime = DateTime.UtcNow,
                Status = Core.Enums.ReservationStatus.Pending,
                Tickets = new List<Ticket>()
            };
            
            foreach(var seatId in reservationDto.SeatIds)
            {
                reservation.Tickets.Add(new Ticket
                {
                    SeatId = seatId,
                    PurchasePrice = session.Price,
                    Status = Core.Enums.TicketStatus.Pending
                });
            }

            reservation.TotalPrice = reservation.Tickets.Sum(x => x.PurchasePrice);
            await _reservationRepo.CreateAsync(reservation);

            var bookedSeats = await _seatRepo.GetSeatsByIdAsync(reservationDto.SeatIds);

            _backgroundJobClient.Schedule<IReservationJobService>(
                job => job.CancelUnpaidReservationAsync(reservation.Id),
                TimeSpan.FromMinutes(10)
            );

            return Ok(reservation.ToReservationDto(session, bookedSeats));
        }
    }
}

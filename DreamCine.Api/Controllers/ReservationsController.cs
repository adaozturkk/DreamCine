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

        [HttpGet("my-reservations")]
        public async Task<IActionResult> GetUserReservations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID could not be found in the token.");
            }

            var reservations = await _reservationRepo.GetReservationsByUserIdAsync(userId);
            var reservationDtos = reservations.Select(r => r.ToReservationDto(
                r.MovieSession, 
                r.Tickets.Select(t => t.Seat).ToList()));

            return Ok(reservationDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID could not be found in the token.");
            }

            var reservation = await _reservationRepo.GetByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Admin") || reservation.UserId == userId)
            {
                var reservationDto = reservation.ToReservationDto(reservation.MovieSession, reservation.Tickets.Select(t => t.Seat).ToList());
                return Ok(reservationDto);
            }

            return NotFound();
        }

        [HttpPatch("{id:int}/cancel")]
        public async Task<IActionResult> Cancel([FromRoute] int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID could not be found in the token.");
            }

            var reservation = await _reservationRepo.GetByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin") && reservation.UserId != userId)
            {
                return NotFound();
            }

            if (reservation.Status != Core.Enums.ReservationStatus.Pending)
            {
                return BadRequest("Only pending reservations can be cancelled.");
            }

            reservation.Status = Core.Enums.ReservationStatus.Cancelled;
            foreach (var ticket in reservation.Tickets)
            {
                ticket.Status = Core.Enums.TicketStatus.Cancelled;
            }
            await _reservationRepo.UpdateAsync(id, reservation);

            return Ok("Reservation cancelled successfully.");
        }
    }
}

using System.Security.Claims;
using DreamCine.Application.DTOs.Reservation;
using DreamCine.Application.Interfaces;
using DreamCine.Application.Mappers;
using DreamCine.Core.Helpers;
using DreamCine.Core.Interfaces;
using DreamCine.Core.Models;
using FluentValidation;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DreamCine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        public IReservationRepository _reservationRepo;
        public IValidator<CreateReservationDto> _createReservationValidator;
        public IReservationService _reservationService;

        public ReservationsController(IReservationRepository reservationRepo, IReservationService reservationService,
            IValidator<CreateReservationDto> createReservationValidator)
        {
            _reservationRepo = reservationRepo;
            _createReservationValidator = createReservationValidator;
            _reservationService = reservationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto reservationDto)
        {
            var validationResult = await _createReservationValidator.ValidateAsync(reservationDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID could not be found in the token.");
            }

            var result = await _reservationService.CreateReservationAsync(reservationDto, userId);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }
            else
            {
                return StatusCode(result.StatusCode, result.Data);
            }
        }

        [HttpGet("my-reservations")]
        public async Task<IActionResult> GetUserReservations([FromQuery] UserReservationQueryObject query)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID could not be found in the token.");
            }

            var reservations = await _reservationRepo.GetReservationsByUserIdAsync(userId, query);
            var reservationDtos = reservations.Select(r => r.ToReservationDto(
                r.MovieSession,
                r.Tickets.Select(t => t.Seat).ToList())
            );

            return Ok(reservationDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] ReservationQueryObject query)
        {
            var reservation = await _reservationRepo.GetAllWithFilteringAsync(query);
            var reservationDtos = reservation.Select(r => r.ToReservationDto(
                r.MovieSession,
                r.Tickets.Select(t => t.Seat).ToList())
            );

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

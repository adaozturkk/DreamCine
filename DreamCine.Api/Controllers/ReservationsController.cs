using System.Security.Claims;
using DreamCine.Application.DTOs.Reservation;
using DreamCine.Application.Interfaces;
using DreamCine.Core.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DreamCine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        public IValidator<CreateReservationDto> _createReservationValidator;
        public IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService,
            IValidator<CreateReservationDto> createReservationValidator)
        {
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

            var result = await _reservationService.GetReservationsByUserIdAsync(userId, query);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }
            else
            {
                return StatusCode(result.StatusCode, result.Data);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] ReservationQueryObject query)
        {
            var result = await _reservationService.GetAllWithFilteringAsync(query);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }
            else
            {
                return StatusCode(result.StatusCode, result.Data);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID could not be found in the token.");
            }

            var isAdmin = User.IsInRole("Admin");

            var result = await _reservationService.GetByIdAsync(id, userId, isAdmin);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }
            else
            {
                return StatusCode(result.StatusCode, result.Data);
            }
        }

        [HttpPatch("{id:int}/cancel")]
        public async Task<IActionResult> Cancel([FromRoute] int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID could not be found in the token.");
            }

            var isAdmin = User.IsInRole("Admin");

            var result = await _reservationService.CancelReservationAsync(id, userId, isAdmin);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.ErrorMessage);
            }
            else
            {
                return StatusCode(result.StatusCode, result.Data);
            }
        }
    }
}

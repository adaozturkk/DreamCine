using DreamCine.Api.DTOs.Ticket;
using DreamCine.Api.Interfaces;
using DreamCine.Api.Mappers;
using DreamCine.Api.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DreamCine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepo;
        private readonly IMovieSessionRepository _sessionRepo;
        private readonly ISeatRepository _seatRepo;
        private readonly IValidator<CreateTicketDto> _createTicketValidator;

        public TicketsController(ITicketRepository ticketRepo, IMovieSessionRepository sessionRepo,
            ISeatRepository seatRepo, IValidator<CreateTicketDto> createTicketValidator)
        {
            _ticketRepo = ticketRepo;
            _sessionRepo = sessionRepo;
            _seatRepo = seatRepo;
            _createTicketValidator = createTicketValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTicketDto createDto)
        {
            var validationResult = await _createTicketValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID could not be found in the token.");
            }

            var session = await _sessionRepo.GetByIdAsync(createDto.MovieSessionId);
            if (session == null)
            {
                return NotFound("Selected movie session does not exist.");
            }
            var currentPrice = session.Price;

            var seat = await _seatRepo.GetByIdAsync(createDto.SeatId);
            if (seat == null)
            {
                return NotFound("Selected seat does not exist.");
            }

            if (session.ScreenId != seat.ScreenId)
            {
                return BadRequest("Selected seat does not belong to the screen where movie session is playing.");
            }

            var isSeatTaken = await _ticketRepo.IsSeatTakenAsync(createDto.MovieSessionId, createDto.SeatId);
            if (isSeatTaken)
            {
                return BadRequest("This seat is already taken for the selected session.");
            }

            var ticketModel = createDto.ToTicketFromCreateDto(userId, currentPrice);
            var createdTicket = await _ticketRepo.CreateAsync(ticketModel);

            var fullTicket = await _ticketRepo.GetByIdAsync(createdTicket.Id);

            return Ok(fullTicket?.ToTicketDto());
        }

        [HttpGet("my-tickets")]
        public async Task<IActionResult> GetUserTickets()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID could not be found in the token.");
            }

            var tickets = await _ticketRepo.GetTicketsByUserIdAsync(userId);
            var ticketDtos = tickets.Select(t => t.ToTicketDto());

            return Ok(ticketDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _ticketRepo.GetAllAsync();
            var ticketDtos = tickets.Select(t => t.ToTicketDto());

            return Ok(ticketDtos);
        }

        [HttpPut("{id:int}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int id, [FromBody] UpdateTicketStatusDto updateDto)
        {
            var ticket = await _ticketRepo.GetByIdAsync(id);

            if (ticket == null)
            {
                return NotFound("Ticket does not exist.");
            }

            ticket.Status = updateDto.Status;
            var updatedTicket = await _ticketRepo.UpdateAsync(id, ticket);

            return Ok(updatedTicket?.ToTicketDto());
        }
    }
}

using DreamCine.Application.DTOs.Ticket;
using DreamCine.Core.Helpers;
using DreamCine.Core.Interfaces;
using DreamCine.Application.Interfaces;
using DreamCine.Application.Mappers;
using DreamCine.Core.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IPaymentService _paymentService;

        public TicketsController(ITicketRepository ticketRepo, IMovieSessionRepository sessionRepo,
            ISeatRepository seatRepo, IValidator<CreateTicketDto> createTicketValidator, IPaymentService paymentService)
        {
            _ticketRepo = ticketRepo;
            _sessionRepo = sessionRepo;
            _seatRepo = seatRepo;
            _createTicketValidator = createTicketValidator;
            _paymentService = paymentService;
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

            var paymentResult = await _paymentService.ProcessPaymentAsync(createDto.PaymentInfo, currentPrice);

            if (!paymentResult)
            {
                return BadRequest("Payment failed. Please check your card details or limit.");
            }

            var ticketModel = createDto.ToTicketFromCreateDto(currentPrice);
            ticketModel.Status = TicketStatus.Paid;

            var createdTicket = await _ticketRepo.CreateAsync(ticketModel);
            var fullTicket = await _ticketRepo.GetByIdAsync(createdTicket.Id);

            return Ok(fullTicket?.ToTicketDto());
        }

        [HttpGet("my-tickets")]
        public async Task<IActionResult> GetUserTickets([FromQuery] UserTicketQuery query)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID could not be found in the token.");
            }

            var tickets = await _ticketRepo.GetTicketsByUserIdAsync(userId, query);
            var ticketDtos = tickets.Select(t => t.ToTicketDto());

            return Ok(ticketDtos);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] TicketQuery query)
        {
            var tickets = await _ticketRepo.GetAllAsync(query);
            var ticketDtos = tickets.Select(t => t.ToTicketDto());

            return Ok(ticketDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID could not be found in the token.");
            }

            var ticket = await _ticketRepo.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Admin") || ticket.Reservation.UserId == userId)
            {
                var ticketDto = ticket.ToTicketDto();
                return Ok(ticketDto);
            }

            return NotFound();
        }

        // add cancel tickets later
    }
}

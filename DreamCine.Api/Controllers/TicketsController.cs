using System.Security.Claims;
using DreamCine.Application.Mappers;
using DreamCine.Core.Helpers;
using DreamCine.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DreamCine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepo;

        public TicketsController(ITicketRepository ticketRepo)
        {
            _ticketRepo = ticketRepo;
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

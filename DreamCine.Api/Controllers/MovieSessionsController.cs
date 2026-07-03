using DreamCine.Application.DTOs.MovieSession;
using DreamCine.Core.Helpers;
using DreamCine.Core.Interfaces;
using DreamCine.Application.Mappers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DreamCine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MovieSessionsController : ControllerBase
    {
        private readonly IMovieSessionRepository _sessionRepo;
        private readonly IMovieRepository _movieRepo;
        private readonly IScreenRepository _screenRepo;
        private readonly ITicketRepository _ticketRepo;
        private readonly IValidator<CreateMovieSessionDto> _createSessionValidator;
        private readonly IValidator<UpdateMovieSessionDto> _updateSessionValidator;

        public MovieSessionsController(IMovieSessionRepository sessionRepo, ITicketRepository ticketRepo,
            IValidator<CreateMovieSessionDto> createSessionVlidator, IValidator<UpdateMovieSessionDto> updateSessionValidator,
            IMovieRepository movieRepo, IScreenRepository screenRepo)
        {
            _sessionRepo = sessionRepo;
            _createSessionValidator = createSessionVlidator;
            _updateSessionValidator = updateSessionValidator;
            _movieRepo = movieRepo;
            _screenRepo = screenRepo;
            _ticketRepo = ticketRepo;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateMovieSessionDto createDto)
        {
            var validationResult = await _createSessionValidator.ValidateAsync(createDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var movieExists = await _movieRepo.ExistsAsync(createDto.MovieId);

            if (!movieExists)
            {
                return BadRequest("Movie does not exist.");
            }

            var screenExist = await _screenRepo.ExistsAsync(createDto.ScreenId);

            if (!screenExist)
            {
                return BadRequest("Screen does not exist.");
            }

            var movieSessionModel = createDto.ToMovieSessionFromCreateDto();
            await _sessionRepo.CreateAsync(movieSessionModel);
            var createdSession = await _sessionRepo.GetByIdAsync(movieSessionModel.Id);

            return Ok(createdSession?.ToMovieSessionDto());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] MovieSessionQuery query)
        {
            var sessions = await _sessionRepo.GetAllAsync(query);
            var sessionsDto = sessions.Select(s => s.ToMovieSessionDto()).ToList();

            return Ok(sessionsDto);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var session = await _sessionRepo.GetByIdAsync(id);

            if (session == null)
            {
                return NotFound();
            }

            return Ok(session.ToMovieSessionDto());
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateMovieSessionDto sessionDto)
        {
            var validationResult = await _updateSessionValidator.ValidateAsync(sessionDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var movieExists = await _movieRepo.ExistsAsync(sessionDto.MovieId);

            if (!movieExists)
            {
                return BadRequest("Movie does not exist.");
            }

            var screenExist = await _screenRepo.ExistsAsync(sessionDto.ScreenId);

            if (!screenExist)
            {
                return BadRequest("Screen does not exist.");
            }

            var sessionModel = sessionDto.ToMovieSessionFromUpdateDto();
            sessionModel.Id = id;

            var session = await _sessionRepo.UpdateAsync(id, sessionModel);

            if (session == null)
            {
                return NotFound();
            }

            var updatedSession = await _sessionRepo.GetByIdAsync(id);

            return Ok(updatedSession?.ToMovieSessionDto());
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var session = await _sessionRepo.DeleteAsync(id);

            if (session == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("{id:int}/seats")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSessionSeats(int id)
        {
            var session = await _sessionRepo.GetByIdAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            var capacity = session.Screen.Capacity;
            var occupiedSeats = await _ticketRepo.GetOccupiedSeatIdsAsync(session.Id);

            return Ok(new SessionSeatsDto
            {
                Capacity = capacity,
                OccupiedSeats = occupiedSeats
            });
        }
    }
}

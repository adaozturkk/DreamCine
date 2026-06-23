using DreamCine.Api.DTOs.Seat;
using DreamCine.Api.DTOs.Status;
using DreamCine.Api.Helpers;
using DreamCine.Api.Interfaces;
using DreamCine.Api.Mappers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DreamCine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatRepository _seatRepo;
        private readonly IScreenRepository _screenRepo;
        private readonly IValidator<CreateSeatDto> _createSeatValidator;
        private readonly IValidator<UpdateSeatDto> _updateSeatValidator;

        public SeatsController(ISeatRepository seatRepo, IScreenRepository screenRepo,
            IValidator<UpdateSeatDto> updateSeatValidator, IValidator<CreateSeatDto> createSeatValidator)
        {
            _seatRepo = seatRepo;
            _screenRepo = screenRepo;
            _updateSeatValidator = updateSeatValidator;
            _createSeatValidator = createSeatValidator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSeat([FromBody] CreateSeatDto seatDto)
        {
            var validationResult = await _createSeatValidator.ValidateAsync(seatDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (await _screenRepo.GetByIdAsync(seatDto.ScreenId) == null)
            {
                return BadRequest("Screen does not exist.");
            }

            if (await _seatRepo.SeatExistsInScreenAsync(seatDto.SeatNumber, seatDto.ScreenId))
            {
                return BadRequest("Seat already exists in that screen.");
            }

            var seatModel = seatDto.ToSeatFromCreateSeatDto();
            await _seatRepo.CreateAsync(seatModel);

            return Ok(seatModel.ToSeatDto());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] SeatQueryObject query)
        {
            var seats = await _seatRepo.GetAllAsync(query);
            var seatDtos = seats.Select(x => x.ToSeatDto()).ToList();

            return Ok(seatDtos);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var seat = await _seatRepo.GetByIdAsync(id);

            if (seat == null)
            {
                return NotFound();
            }

            return Ok(seat.ToSeatDto());
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSeatDto seatDto)
        {
            var validationResult = await _updateSeatValidator.ValidateAsync(seatDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (await _screenRepo.GetByIdAsync(seatDto.ScreenId) == null)
            {
                return BadRequest("Screen does not exist.");
            }

            if (await _seatRepo.SeatExistsInScreenAsync(seatDto.SeatNumber, seatDto.ScreenId, id))
            {
                return BadRequest("Seat already exists in that screen.");
            }

            var seatModel = seatDto.ToSeatFromUpdateSeatDto();
            var updatedSeat = await _seatRepo.UpdateAsync(id, seatModel);

            if (updatedSeat == null)
            {
                return NotFound();
            }

            return Ok(updatedSeat.ToSeatDto());
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var seat = await _seatRepo.DeleteAsync(id);

            if (seat == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

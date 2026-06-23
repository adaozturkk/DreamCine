using DreamCine.Api.DTOs.Screen;
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
    public class ScreensController : ControllerBase
    {
        private readonly IScreenRepository _screenRepo;
        private readonly IValidator<CreateScreenDto> _createScreenValidator;
        private readonly IValidator<UpdateScreenDto> _updateScreenValidator;

        public ScreensController(IScreenRepository screenRepo, IValidator<CreateScreenDto> createScreenValidator, IValidator<UpdateScreenDto> updateScreenValidator)
        {
            _screenRepo = screenRepo;
            _createScreenValidator = createScreenValidator;
            _updateScreenValidator = updateScreenValidator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateScreen([FromBody] CreateScreenDto screenDto)
        {
            var validationResult = await _createScreenValidator.ValidateAsync(screenDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (await _screenRepo.ScreenNumberExistsAsync(screenDto.ScreenNumber))
            {
                return BadRequest("This screen number already exists.");
            }

            var screen = await _screenRepo.CreateAsync(screenDto.ToScreenFromCreateDto());
            return Ok(screen.ToScreenDto());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var screens = await _screenRepo.GetAllAsync();
            var screenDtos = screens.Select(x => x.ToScreenDto()).ToList();

            return Ok(screenDtos);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var screen = await _screenRepo.GetByIdAsync(id);

            if (screen == null)
            {
                return NotFound();
            }

            return Ok(screen.ToScreenDto());
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateScreenDto updateDto)
        {
            var validationResult = await _updateScreenValidator.ValidateAsync(updateDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (await _screenRepo.ScreenNumberExistsAsync(updateDto.ScreenNumber, id))
            {
                return BadRequest("This screen number already exists.");
            }

            var screen = await _screenRepo.UpdateAsync(id, updateDto.ToScreenFromUpdateDto());

            if (screen == null)
            {
                return NotFound();
            }

            return Ok(screen.ToScreenDto());
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var screen = await _screenRepo.DeleteAsync(id);

            if (screen == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

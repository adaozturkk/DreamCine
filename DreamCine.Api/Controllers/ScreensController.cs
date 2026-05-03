using DreamCine.Api.DTOs.Screen;
using DreamCine.Api.Interfaces;
using DreamCine.Api.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DreamCine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreensController : ControllerBase
    {
        private readonly IScreenRepository _screenRepo;

        public ScreensController(IScreenRepository screenRepo)
        {
            _screenRepo = screenRepo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateScreen([FromBody] CreateScreenDto screenDto)
        {
            if (await _screenRepo.ScreenNumberExistsAsync(screenDto.ScreenNumber))
            {
                return BadRequest("This screen number already exists.");
            }

            var screen = await _screenRepo.CreateAsync(screenDto.ToScreenFromCreateDto());
            return Ok(screen.ToScreenDto());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var screens = await _screenRepo.GetAllAsync();
            var screenDtos = screens.Select(x => x.ToScreenDto()).ToList();

            return Ok(screenDtos);
        }

        [HttpGet("{id:int}")]
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
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateScreenDto updateDto)
        {
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

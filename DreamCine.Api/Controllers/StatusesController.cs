using DreamCine.Api.DTOs.Status;
using DreamCine.Api.Interfaces;
using DreamCine.Api.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DreamCine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusesController : ControllerBase
    {
        private readonly IStatusRepository _statusRepo;

        public StatusesController(IStatusRepository statusRepo)
        {
            _statusRepo = statusRepo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStatus([FromBody] CreateStatusDto statusDto)
        {
            if (await _statusRepo.NameExistsAsync(statusDto.Name))
            {
                return BadRequest("Status name already exists.");
            }

            var statusModel = statusDto.ToStatusFromCreateDto();
            await _statusRepo.CreateAsync(statusModel);

            return Ok(statusModel.ToStatusDto());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var statuses = await _statusRepo.GetAllAsync();
            var statusDtos = statuses.Select(x => x.ToStatusDto()).ToList();

            return Ok(statusDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var status = await _statusRepo.GetByIdAsync(id);

            if (status == null)
            {
                return NotFound();
            }

            return Ok(status.ToStatusDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStatusDto statusDto)
        {
            if (await _statusRepo.NameExistsAsync(statusDto.Name, id))
            {
                return BadRequest("Status name already exists.");
            }

            var status = await _statusRepo.UpdateAsync(id, statusDto.ToStatusFromUpdateDto());

            if (status == null)
            {
                return NotFound();
            }

            return Ok(status.ToStatusDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var status = await _statusRepo.DeleteAsync(id);

            if (status == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

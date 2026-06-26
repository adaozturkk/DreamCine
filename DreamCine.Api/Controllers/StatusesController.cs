using DreamCine.Api.DTOs.Movie;
using DreamCine.Api.DTOs.Status;
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
    public class StatusesController : ControllerBase
    {
        private readonly IStatusRepository _statusRepo;
        private readonly IValidator<CreateStatusDto> _createStatusValidator;
        private readonly IValidator<UpdateStatusDto> _updateStatusValidator;

        public StatusesController(IStatusRepository statusRepo, IValidator<CreateStatusDto> createStatusValidator, IValidator<UpdateStatusDto> updateStatusValidator)
        {
            _statusRepo = statusRepo;
            _createStatusValidator = createStatusValidator;
            _updateStatusValidator = updateStatusValidator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateStatus([FromBody] CreateStatusDto statusDto)
        {
            var validationResult = await _createStatusValidator.ValidateAsync(statusDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (await _statusRepo.NameExistsAsync(statusDto.Name))
            {
                return BadRequest("Status name already exists.");
            }

            var statusModel = statusDto.ToStatusFromCreateDto();
            await _statusRepo.CreateAsync(statusModel);

            return Ok(statusModel.ToStatusDto());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var statuses = await _statusRepo.GetAllAsync();
            var statusDtos = statuses.Select(x => x.ToStatusDto()).ToList();

            return Ok(statusDtos);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStatusDto statusDto)
        {
            var validationResult = await _updateStatusValidator.ValidateAsync(statusDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (await _statusRepo.NameExistsAsync(statusDto.Name, id))
            {
                return BadRequest("Status name already exists.");
            }

            var statusModel = statusDto.ToStatusFromUpdateDto();
            statusModel.Id = id;

            var status = await _statusRepo.UpdateAsync(id, statusModel);

            if (status == null)
            {
                return NotFound();
            }

            return Ok(status.ToStatusDto());
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
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

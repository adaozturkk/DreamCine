using DreamCine.Api.Data;
using DreamCine.Api.DTOs.Genre;
using DreamCine.Api.Interfaces;
using DreamCine.Api.Mappers;
using DreamCine.Api.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreRepository _genreRepo;
        private readonly IValidator<CreateGenreDto> _createGenreValidator;
        private readonly IValidator<UpdateGenreDto> _updateGenreValidator;

        public GenresController(IGenreRepository genreRepo, IValidator<CreateGenreDto> createGenreValidator, IValidator<UpdateGenreDto> updateGenreValidator)
        {
            _genreRepo = genreRepo;
            _createGenreValidator = createGenreValidator;
            _updateGenreValidator = updateGenreValidator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromBody] CreateGenreDto createGenreDto)
        {
            var validationResult = await _createGenreValidator.ValidateAsync(createGenreDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (await _genreRepo.NameExistsAsync(createGenreDto.Name))
            {
                return BadRequest("Genre name already exists.");
            }

            var genreModel = createGenreDto.ToGenreFromCreateDto();
            await _genreRepo.CreateAsync(genreModel);

            return Ok(genreModel.ToGenreDto());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _genreRepo.GetAllAsync();
            var genreDtos = genres.Select(g => g.ToGenreDto()).ToList();

            return Ok(genreDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var genre = await _genreRepo.GetByIdAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return Ok(genre.ToGenreDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateGenreDto updateDto)
        {
            var validationResult = await _updateGenreValidator.ValidateAsync(updateDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (await _genreRepo.NameExistsAsync(updateDto.Name, id))
            {
                return BadRequest("Genre name already exists.");
            }

            var genreModel = new Genre { Name = updateDto.Name };
            var updatedGenre = await _genreRepo.UpdateAsync(id, genreModel);

            if (updatedGenre == null)
            {
                return NotFound();
            }

            return Ok(updatedGenre.ToGenreDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deletedGenre = await _genreRepo.DeleteAsync(id);

            if (deletedGenre == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

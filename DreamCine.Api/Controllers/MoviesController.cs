using DreamCine.Api.DTOs.Movie;
using DreamCine.Api.Helpers;
using DreamCine.Api.Interfaces;
using DreamCine.Api.Mappers;
using DreamCine.Api.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DreamCine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepo;
        private readonly IGenreRepository _genreRepo;
        private readonly IStatusRepository _statusRepo;
        private readonly IValidator<CreateMovieDto> _createMovieValidator;
        private readonly IValidator<UpdateMovieDto> _updateMovieValidator;

        public MoviesController(IMovieRepository movieRepo, IGenreRepository genreRepo, IStatusRepository statusRepo,
            IValidator<CreateMovieDto> createMovieValidator, IValidator<UpdateMovieDto> updateMovieValidator)
        {
            _movieRepo = movieRepo;
            _genreRepo = genreRepo;
            _statusRepo = statusRepo;
            _createMovieValidator = createMovieValidator;
            _updateMovieValidator = updateMovieValidator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieDto createMovieDto)
        {
            var validationResult = await _createMovieValidator.ValidateAsync(createMovieDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            bool genresExist = await _genreRepo.DoGenresExistAsync(createMovieDto.GenreIds);

            if (!genresExist)
            {
                return BadRequest("Genre types are not valid.");
            }

            bool statusExist = await _statusRepo.StatusExistsAsync(createMovieDto.StatusId);

            if (!statusExist)
            {
                return BadRequest("Selected status does not exist.");
            }

            var movieModel = createMovieDto.ToMovieFromCreateDto();
            await _movieRepo.CreateAsync(movieModel);
            var createdMovie = await _movieRepo.GetByIdAsync(movieModel.Id);

            return Ok(createdMovie!.ToMovieDto());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] MovieQueryObject query)
        {
            var moviesModel = await _movieRepo.GetAllWithFilteringAsync(query);
            var moviesDto = moviesModel.Select(x => x.ToMovieDto()).ToList();

            return Ok(moviesDto);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var movie = await _movieRepo.GetByIdAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie.ToMovieDto());
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateMovieDto movieDto)
        {
            var validationResult = await _updateMovieValidator.ValidateAsync(movieDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            bool genresExist = await _genreRepo.DoGenresExistAsync(movieDto.GenreIds);

            if (!genresExist)
            {
                return BadRequest("Genre types are not valid.");
            }

            bool statusExist = await _statusRepo.StatusExistsAsync(movieDto.StatusId);

            if (!statusExist)
            {
                return BadRequest("Selected status does not exist.");
            }

            var movieModel = movieDto.ToMovieFromUpdateDto();
            movieModel.Id = id;

            var movie = await _movieRepo.UpdateAsync(id, movieModel);

            if (movie == null)
            {
                return NotFound();
            }

            var updatedMovie = await _movieRepo.GetByIdAsync(id);

            return Ok(updatedMovie!.ToMovieDto());
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var movie = await _movieRepo.DeleteAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

using DreamCine.Application.DTOs.Movie;
using DreamCine.Core.Helpers;
using DreamCine.Core.Interfaces;
using DreamCine.Application.Mappers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DreamCine.Application.Interfaces;

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
        private readonly IFileService _fileService;

        public MoviesController(IMovieRepository movieRepo, IGenreRepository genreRepo, IStatusRepository statusRepo,
            IValidator<CreateMovieDto> createMovieValidator, IValidator<UpdateMovieDto> updateMovieValidator,
            IFileService fileService)
        {
            _movieRepo = movieRepo;
            _genreRepo = genreRepo;
            _statusRepo = statusRepo;
            _createMovieValidator = createMovieValidator;
            _updateMovieValidator = updateMovieValidator;
            _fileService = fileService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMovie([FromForm] CreateMovieDto createMovieDto)
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

            if (createMovieDto.PosterImage != null)
            {
                string fileName = await _fileService.UploadFileAsync(createMovieDto.PosterImage, "images/posters");
                movieModel.PosterPath = "images/posters/" + fileName;
            }
            
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

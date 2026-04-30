using DreamCine.Api.DTOs.Movie;
using DreamCine.Api.Interfaces;
using DreamCine.Api.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DreamCine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepo;
        private readonly IGenreRepository _genreRepo;

        public MoviesController(IMovieRepository movieRepo, IGenreRepository genreRepo)
        {
            _movieRepo = movieRepo;
            _genreRepo = genreRepo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieDto createMovieDto)
        {
            if (createMovieDto.GenreIds.Any())
            {
                bool genresExist = await _genreRepo.DoGenresExistAsync(createMovieDto.GenreIds);

                if (!genresExist)
                {
                    return BadRequest("Genre types are not valid.");
                }
            }

            var movieModel = createMovieDto.ToMovieFromCreateDto();
            await _movieRepo.CreateAsync(movieModel);

            return Ok(movieModel.ToMovieDto());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var moviesModel = await _movieRepo.GetAllAsync();
            var moviesDto = moviesModel.Select(x => x.ToMovieDto()).ToList();

            return Ok(moviesDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var movie = await _movieRepo.GetByIdAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie.ToMovieDto());
        }
    }
}

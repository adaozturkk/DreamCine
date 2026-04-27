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

        public MoviesController(IMovieRepository movieRepo)
        {
            _movieRepo = movieRepo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieDto createMovieDto)
        {
            var movieModel = createMovieDto.ToMovieFromCreateDto();
            await _movieRepo.CreateAsync(movieModel);

            return Ok(movieModel);
        }
    }
}

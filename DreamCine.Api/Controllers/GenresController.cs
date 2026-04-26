using DreamCine.Api.Data;
using DreamCine.Api.DTOs.Genre;
using DreamCine.Api.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GenresController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromBody] CreateGenreDto createGenreDto)
        {
            var genreModel = createGenreDto.ToGenreFromCreateDto();

            await _context.AddAsync(genreModel);
            await _context.SaveChangesAsync();

            return Ok(genreModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _context.Genres.ToListAsync();
            var genreDtos = genres.Select(g => g.ToGenreDto()).ToList();

            return Ok(genreDtos);
        }
    }
}

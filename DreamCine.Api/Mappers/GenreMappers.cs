using DreamCine.Api.DTOs.Genre;
using DreamCine.Api.Models;

namespace DreamCine.Api.Mappers
{
    public static class GenreMappers
    {
        public static Genre ToGenreFromCreateDto(this CreateGenreDto genreDto)
        {
            return new Genre
            {
                Name = genreDto.Name
            };
        }
    }
}

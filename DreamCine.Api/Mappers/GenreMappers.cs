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

        public static GenreDto ToGenreDto(this Genre genreModel)
        {
            return new GenreDto
            {
                Id = genreModel.Id,
                Name = genreModel.Name
            };
        }
    }
}

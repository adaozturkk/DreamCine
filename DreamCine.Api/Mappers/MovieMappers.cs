using DreamCine.Api.DTOs.Movie;
using DreamCine.Api.Models;

namespace DreamCine.Api.Mappers
{
    public static class MovieMappers
    {
        public static Movie ToMovieFromCreateDto(this CreateMovieDto movieDto)
        {
            return new Movie
            {
                Title = movieDto.Title,
                Description = movieDto.Description,
                Duration = movieDto.Duration,
                Rating = movieDto.Rating,
                ReleaseDate = movieDto.ReleaseDate,
            };
        }
    }
}

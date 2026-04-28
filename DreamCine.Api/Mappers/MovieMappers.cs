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
                MovieGenres = movieDto.GenreIds.Select(id => new MovieGenre {  GenreId = id }).ToList()
            };
        }

        public static MovieDto ToMovieDto(this Movie movieModel)
        {
            return new MovieDto
            {
                Id = movieModel.Id,
                Title = movieModel.Title,
                Description = movieModel.Description,
                Duration = movieModel.Duration,
                Rating = movieModel.Rating,
                ReleaseDate = movieModel.ReleaseDate,
            };
        }
    }
}

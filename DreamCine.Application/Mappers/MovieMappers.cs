using DreamCine.Application.DTOs.Movie;
using DreamCine.Core.Models;

namespace DreamCine.Application.Mappers
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
                MovieGenres = movieDto.GenreIds.Select(id => new MovieGenre { GenreId = id }).ToList(),
                StatusId = movieDto.StatusId
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
                Genres = movieModel.MovieGenres.Select(x => x.Genre.Name).ToList(),
                StatusId = movieModel.StatusId,
                StatusName = movieModel.Status?.Name ?? string.Empty,
                PosterPath = movieModel.PosterPath
            };
        }

        public static Movie ToMovieFromUpdateDto(this UpdateMovieDto movieDto)
        {
            return new Movie
            {
                Title = movieDto.Title,
                Description = movieDto.Description,
                Duration = movieDto.Duration,
                Rating = movieDto.Rating,
                ReleaseDate = movieDto.ReleaseDate,
                MovieGenres = movieDto.GenreIds.Select(id => new MovieGenre { GenreId = id }).ToList(),
                StatusId = movieDto.StatusId
            };
        }
    }
}

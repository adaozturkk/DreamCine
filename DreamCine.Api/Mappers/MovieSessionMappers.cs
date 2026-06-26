using DreamCine.Api.DTOs.MovieSession;
using DreamCine.Api.Models;

namespace DreamCine.Api.Mappers
{
    public static class MovieSessionMappers
    {
        public static MovieSession ToMovieSessionFromCreateDto(this CreateMovieSessionDto createDto)
        {
            return new MovieSession
            {
                MovieId = createDto.MovieId,
                ScreenId = createDto.ScreenId,
                SessionTime = createDto.SessionTime,
                Price = createDto.Price
            };
        }

        public static MovieSessionDto ToMovieSessionDto(this MovieSession movieSessionModel)
        {
            return new MovieSessionDto
            {
                Id = movieSessionModel.Id,
                MovieId = movieSessionModel.MovieId,
                MovieName = movieSessionModel.Movie?.Title,
                ScreenId = movieSessionModel.ScreenId,
                ScreenNumber = movieSessionModel.Screen?.ScreenNumber,
                SessionTime = movieSessionModel.SessionTime,
                Price = movieSessionModel.Price
            };
        }

        public static MovieSession ToMovieSessionFromUpdateDto(this UpdateMovieSessionDto updateDto)
        {
            return new MovieSession
            {
                MovieId = updateDto.MovieId,
                ScreenId = updateDto.ScreenId,
                SessionTime = updateDto.SessionTime,
                Price = updateDto.Price
            };
        }
    }
}

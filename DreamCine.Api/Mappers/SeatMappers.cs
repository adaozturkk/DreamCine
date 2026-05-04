using DreamCine.Api.DTOs.Seat;
using DreamCine.Api.Models;

namespace DreamCine.Api.Mappers
{
    public static class SeatMappers
    {
        public static Seat ToSeatFromCreateSeatDto(this CreateSeatDto seatDto)
        {
            return new Seat
            {
                ScreenId = seatDto.ScreenId,
                SeatNumber = seatDto.SeatNumber
            };
        }

        public static SeatDto ToSeatDto(this Seat seatModel)
        {
            return new SeatDto
            {
                Id = seatModel.Id,
                ScreenId = seatModel.ScreenId,
                SeatNumber = seatModel.SeatNumber
            };
        }

        public static Seat ToSeatFromUpdateSeatDto(this UpdateSeatDto seatDto)
        {
            return new Seat
            {
                ScreenId = seatDto.ScreenId,
                SeatNumber = seatDto.SeatNumber
            };
        }
    }
}

using DreamCine.Application.DTOs.Seat;
using DreamCine.Core.Models;

namespace DreamCine.Application.Mappers
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

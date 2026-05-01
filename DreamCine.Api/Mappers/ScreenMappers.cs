using DreamCine.Api.DTOs.Screen;
using DreamCine.Api.Models;

namespace DreamCine.Api.Mappers
{
    public static class ScreenMappers
    {
        public static Screen ToScreenFromCreateDto(this CreateScreenDto screenDto)
        {
            return new Screen
            {
                ScreenNumber = screenDto.ScreenNumber,
                Capacity = screenDto.Capacity
            };
        }

        public static ScreenDto ToScreenDto(this Screen screenModel)
        {
            return new ScreenDto
            {
                Id = screenModel.Id,
                ScreenNumber = screenModel.ScreenNumber,
                Capacity = screenModel.Capacity
            };
        }

        public static Screen ToScreenFromUpdateDto(this UpdateScreenDto screenDto)
        {
            return new Screen
            {
                ScreenNumber = screenDto.ScreenNumber,
                Capacity = screenDto.Capacity
            };
        }
    }
}

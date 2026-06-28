using DreamCine.Application.DTOs.Status;
using DreamCine.Core.Models;

namespace DreamCine.Application.Mappers
{
    public static class StatusMappers
    {
        public static Status ToStatusFromCreateDto(this CreateStatusDto statusDto)
        {
            return new Status
            {
                Name = statusDto.Name,
            };
        }

        public static StatusDto ToStatusDto(this Status statusModel)
        {
            return new StatusDto
            {
                Id = statusModel.Id,
                Name = statusModel.Name
            };
        }

        public static Status ToStatusFromUpdateDto(this UpdateStatusDto statusDto)
        {
            return new Status
            {
                Name = statusDto.Name
            };
        }
    }
}

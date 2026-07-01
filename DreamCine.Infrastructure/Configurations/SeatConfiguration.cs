using DreamCine.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DreamCine.Infrastructure.Configurations
{
    public class SeatConfiguration : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            var seats = new List<Seat>();
            int seatId = 1;

            string[] screen1Rows = { "A", "B", "C" };
            foreach (var row in screen1Rows)
            {
                for (int i = 1; i <= 10; i++)
                {
                    seats.Add(new Seat { Id = seatId++, ScreenId = 1, SeatNumber = $"{row}{i}" });
                }
            }

            string[] screen2Rows = { "A", "B" };
            foreach (var row in screen2Rows)
            {
                for (int i = 1; i <= 10; i++)
                {
                    seats.Add(new Seat { Id = seatId++, ScreenId = 2, SeatNumber = $"{row}{i}" });
                }
            }

            builder.HasData(seats);
        }
    }
}

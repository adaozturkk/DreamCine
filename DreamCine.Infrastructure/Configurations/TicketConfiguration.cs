using DreamCine.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DreamCine.Infrastructure.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder
                .Property(t => t.PurchasePrice)
                .HasPrecision(18, 2);

            builder
                .HasOne(t => t.Seat)
                .WithMany()
                .HasForeignKey(t => t.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(t => t.Reservation)
                .WithMany(x => x.Tickets)
                .HasForeignKey(t => t.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(t => t.MovieSession)
                .WithMany()
                .HasForeignKey(t => t.MovieSessionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasIndex(t => new { t.MovieSessionId, t.SeatId })
                .IsUnique()
                .HasFilter("[Status] IN (1, 2)");
        }
    }
}

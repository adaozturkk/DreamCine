using DreamCine.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DreamCine.Infrastructure.Configurations
{
    public class MovieSessionConfiguration : IEntityTypeConfiguration<MovieSession>
    {
        public void Configure(EntityTypeBuilder<MovieSession> builder)
        {
            builder
                .Property(m => m.Price)
                .HasPrecision(18, 2);
        }
    }
}

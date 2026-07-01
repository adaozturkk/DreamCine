using DreamCine.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DreamCine.Infrastructure.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.ReleaseDate);
            builder
                .Property(x => x.Rating)
                .HasPrecision(3, 1);
            builder.HasData(
                new Movie
                {
                    Id = 1,
                    Title = "Dune: Part Two",
                    Description = "Paul Atreides unites with Chani...",
                    Duration = 166,
                    Rating = 8.8m,
                    ReleaseDate = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    StatusId = 1
                },
                new Movie
                {
                    Id = 2,
                    Title = "Joker: Folie à Deux",
                    Description = "Arthur Fleck is institutionalized...",
                    Duration = 138,
                    Rating = 7.5m,
                    ReleaseDate = new DateTime(2024, 10, 4, 0, 0, 0, DateTimeKind.Utc),
                    StatusId = 2
                },
                new Movie
                {
                    Id = 3,
                    Title = "Deadpool & Wolverine",
                    Description = "The irresponsible hero Deadpool...",
                    Duration = 127,
                    Rating = 8.1m,
                    ReleaseDate = new DateTime(2024, 7, 26, 0, 0, 0, DateTimeKind.Utc),
                    StatusId = 1
                }
            );
        }
    }
}

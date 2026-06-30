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
        }
    }
}

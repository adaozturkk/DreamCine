using DreamCine.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DreamCine.Infrastructure.Configurations
{
    public class ScreenConfiguration : IEntityTypeConfiguration<Screen>
    {
        public void Configure(EntityTypeBuilder<Screen> builder)
        {
            builder.HasData(
                new Screen { Id = 1, ScreenNumber = 1, Capacity = 30 },
                new Screen { Id = 2, ScreenNumber = 2, Capacity = 20 }
            );
        }
    }
}

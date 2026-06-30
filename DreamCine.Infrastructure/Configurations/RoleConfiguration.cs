using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DreamCine.Infrastructure.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole { 
                    Id = "297de5bf-b618-40c6-b2f3-e36a56d1971e", 
                    Name = "Admin", 
                    NormalizedName = "ADMIN" 
                },
                new IdentityRole { 
                    Id = "3dfcb2e7-0d2a-476a-873d-694b87f5f238", 
                    Name = "User", 
                    NormalizedName = "USER" 
                },
                new IdentityRole {
                    Id = "9c2cc2e8-ae82-4245-8774-edf5f34d6833",
                    Name = "Staff",
                    NormalizedName = "STAFF"
                }
            };

            builder.HasData(roles);
        }
    }
}

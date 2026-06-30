using DreamCine.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DreamCine.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<MovieSession> MovieSessions { get; set; }
        public DbSet<Screen> Screens { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });


            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "User", NormalizedName = "USER" }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            modelBuilder.Entity<MovieSession>()
                .Property(m => m.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Ticket>()
                .Property(t => t.PurchasePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Seat)
                .WithMany()
                .HasForeignKey(t => t.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.MovieSession)
                .WithMany()
                .HasForeignKey(t => t.MovieSessionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

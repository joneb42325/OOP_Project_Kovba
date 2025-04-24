using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OOP_Project_Kovba.Models;

namespace MyMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Trip>()
                .HasOne(d => d.Driver)
                .WithMany(p => p.trips)
                .HasForeignKey(a => a.DriverId)
                .OnDelete(DeleteBehavior.Cascade);
            /*
            builder.Entity<Trip>()
                 .HasMany<Booking>()
                 .WithOne(b => b.Trip)
                 .HasForeignKey(b => b.TripId)
                 .OnDelete(DeleteBehavior.Cascade);
            */
            builder.Entity<Booking>()
                .HasOne(u => u.User)
                .WithMany(b => b.bookings)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<Booking>()
                .HasOne(t => t.Trip)
                .WithMany(t => t.Bookings)
                .HasForeignKey(c => c.TripId)
                .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}


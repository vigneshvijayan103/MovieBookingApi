using Microsoft.EntityFrameworkCore;
using MovieTicketBookingAPI.Models;


namespace MovieTicketBookingAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        
        public DbSet<Seat> Seats { get; set; }

        public DbSet<Show> Shows { get; set; }
        public DbSet<Booking> Bookings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Seat>(entity =>
            {
               
                entity.Property(s => s.RowVersion)
                      .IsRowVersion()
                      .IsConcurrencyToken();

              
                entity.HasIndex(s => new { s.ShowId, s.SeatNumber })
                      .IsUnique();

              
                entity.HasIndex(s => new { s.ShowId, s.Status });
            });
        }

    }


}

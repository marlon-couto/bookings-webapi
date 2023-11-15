using BookingsWebApi.Data;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Repositories
{
    public class BookingsDbContext : DbContext, IBookingsDbContext
    {
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<Hotel> Hotels { get; set; } = null!;
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public BookingsDbContext(DbContextOptions<BookingsDbContext> options) : base(options)
        { }
        public BookingsDbContext()
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Data Source=Data.db";
            optionsBuilder.UseSqlite(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .HasOne(booking => booking.User)
                .WithMany(user => user.Bookings)
                .HasForeignKey(booking => booking.UserId);

            modelBuilder.Entity<Booking>()
                .HasOne(booking => booking.Room)
                .WithMany(room => room.Bookings)
                .HasForeignKey(booking => booking.RoomId);

            modelBuilder.Entity<Room>()
                .HasOne(room => room.Hotel)
                .WithMany(hotel => hotel.Rooms)
                .HasForeignKey(room => room.HotelId);

            modelBuilder.Entity<Hotel>()
                .HasOne(hotel => hotel.City)
                .WithMany(city => city.Hotels)
                .HasForeignKey(hotel => hotel.CityId);

            modelBuilder.SeedData();
        }
    }
}

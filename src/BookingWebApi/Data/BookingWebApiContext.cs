using BookingWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingWebApi.Data
{
    public class BookingWebApiContext : DbContext, IBookingWebApiContext
    {
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }
        public BookingWebApiContext(DbContextOptions<BookingWebApiContext> options) : base(options)
        { }
        public BookingWebApiContext()
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Data Source=Data.db";
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

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Email = "user1@mail.com",
                    Name = "User 1",
                    Password = "Pass1",
                    Role = "Client",
                    UserId = "1"
                }
            );

            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Capacity = 2,
                    HotelId = "1",
                    Image = "Image 1",
                    Name = "Room 1",
                    RoomId = "1"
                }
            );

            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    BookingId = "1",
                    CheckIn = new DateTime(2023, 11, 7),
                    CheckOut = new DateTime(2023, 11, 8),
                    GuestQuantity = 2,
                    RoomId = "1",
                    UserId = "1"
                }
            );
        }
    }
}

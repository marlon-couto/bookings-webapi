using BookingWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingWebApi.Repositories
{
    public interface IBookingWebApiContext
    {
        public int SaveChanges();
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

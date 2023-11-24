using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Repositories;

public interface IBookingsDbContext
{
    DbSet<Booking> Bookings { get; set; }
    DbSet<City> Cities { get; set; }
    DbSet<Hotel> Hotels { get; set; }
    DbSet<Room> Rooms { get; set; }
    DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync();
}
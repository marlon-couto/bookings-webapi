using BookingsWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Repositories;

public interface IBookingsDbContext
{
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<User> Users { get; set; }
    public Task<int> SaveChangesAsync();
}

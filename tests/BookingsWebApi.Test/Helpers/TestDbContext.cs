using System.Threading.Tasks;
using BookingsWebApi.Context;
using BookingsWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Test.Context;

public class TestDbContext : DbContext, IBookingsDbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options) { }

    public DbSet<Booking> Bookings { get; set; } = null!;
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<Hotel> Hotels { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }
}

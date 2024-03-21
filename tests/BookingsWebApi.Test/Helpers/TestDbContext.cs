using System.Threading.Tasks;

using BookingsWebApi.Context;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Test.Helpers;

public class TestDbContext : DbContext, IBookingsDbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public DbSet<BookingModel> Bookings { get; set; } = null!;
    public DbSet<CityModel> Cities { get; set; } = null!;
    public DbSet<HotelModel> Hotels { get; set; } = null!;
    public DbSet<RoomModel> Rooms { get; set; } = null!;
    public DbSet<UserModel> Users { get; set; } = null!;

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }
}
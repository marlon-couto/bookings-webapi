using System.Threading.Tasks;

using BookingsWebApi.Context;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Test.Helpers;

public class TestDbContext : DbContext, IBookingsDbContext
{
    public TestDtContext(DbContextOptions<TestDbContext> opts)
        : base(opts)
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
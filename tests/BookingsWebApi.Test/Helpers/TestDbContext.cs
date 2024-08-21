using System.Threading.Tasks;
using BookingsWebApi.Context;
using BookingsWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Test.Helpers;

public class TestDbContext : DbContext, IBookingsDbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> opts)
        : base(opts) { }

    public DbSet<BookingModel> Bookings { get; set; } = null!;
    public DbSet<CityModel> Cities { get; set; } = null!;
    public DbSet<HotelModel> Hotels { get; set; } = null!;
    public DbSet<RoomModel> Rooms { get; set; } = null!;
    public DbSet<UserModel> Users { get; set; } = null!;

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // PKs
        modelBuilder.Entity<BookingModel>().HasKey(x => x.Id);
        modelBuilder.Entity<RoomModel>().HasKey(x => x.Id);
        modelBuilder.Entity<HotelModel>().HasKey(x => x.Id);
        modelBuilder.Entity<CityModel>().HasKey(x => x.Id);
        modelBuilder.Entity<UserModel>().HasKey(x => x.Id);

        // Relations
        modelBuilder
            .Entity<BookingModel>()
            .HasOne(x => x.User)
            .WithMany(y => y.Bookings)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder
            .Entity<BookingModel>()
            .HasOne(x => x.Room)
            .WithMany(y => y.Bookings)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder
            .Entity<RoomModel>()
            .HasOne(x => x.Hotel)
            .WithMany(y => y.Rooms)
            .HasForeignKey(x => x.HotelId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder
            .Entity<HotelModel>()
            .HasOne(x => x.City)
            .WithMany(y => y.Hotels)
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

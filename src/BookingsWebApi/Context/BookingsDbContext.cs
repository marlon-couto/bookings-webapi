using BookingsWebApi.Models;
using dotenv.net.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Context;

public class BookingsDbContext : DbContext, IBookingsDbContext
{
    public BookingsDbContext(DbContextOptions<BookingsDbContext> opts)
        : base(opts) { }

    public BookingsDbContext() { }

    public DbSet<BookingModel> Bookings { get; set; } = null!;
    public DbSet<CityModel> Cities { get; set; } = null!;
    public DbSet<HotelModel> Hotels { get; set; } = null!;
    public DbSet<RoomModel> Rooms { get; set; } = null!;
    public DbSet<UserModel> Users { get; set; } = null!;

    public Task<int> SaveChangesAsync()
    {
        return base.SaveChangesAsync();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = EnvReader.GetStringValue("CONNECTION_STRING");
        optionsBuilder.UseNpgsql(
            connectionString,
            opts => opts.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
        );
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
            .HasForeignKey(x => x.UserId);
        modelBuilder
            .Entity<BookingModel>()
            .HasOne(x => x.Room)
            .WithMany(y => y.Bookings)
            .HasForeignKey(x => x.RoomId);
        modelBuilder
            .Entity<RoomModel>()
            .HasOne(x => x.Hotel)
            .WithMany(y => y.Rooms)
            .HasForeignKey(x => x.HotelId);
        modelBuilder
            .Entity<HotelModel>()
            .HasOne(x => x.City)
            .WithMany(y => y.Hotels)
            .HasForeignKey(x => x.CityId);

        // Seeder
        modelBuilder.SeedData();
    }
}

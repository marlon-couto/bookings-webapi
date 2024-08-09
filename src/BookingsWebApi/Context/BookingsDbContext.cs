using BookingsWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Context;

public class BookingsDbContext : DbContext, IBookingsDbContext
{
    private readonly IConfiguration? _config;

    public BookingsDbContext(
        DbContextOptions<BookingsDbContext> opts,
        IConfiguration config
    )
        : base(opts)
    {
        _config = config;
    }

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
        var connectionString = _config?["ConnectionStrings:Database"] ?? string.Empty;
        optionsBuilder.UseNpgsql(connectionString,
            opts => opts.EnableRetryOnFailure(
                5,
                TimeSpan.FromSeconds(10),
                null));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // PKs
        modelBuilder.Entity<BookingModel>().HasKey(b => b.Id);
        modelBuilder.Entity<RoomModel>().HasKey(r => r.Id);
        modelBuilder.Entity<HotelModel>().HasKey(h => h.Id);
        modelBuilder.Entity<CityModel>().HasKey(c => c.Id);
        modelBuilder.Entity<UserModel>().HasKey(u => u.Id);

        // Relations
        modelBuilder
            .Entity<BookingModel>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId);
        modelBuilder
            .Entity<BookingModel>()
            .HasOne(b => b.Room)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.RoomId);
        modelBuilder
            .Entity<RoomModel>()
            .HasOne(r => r.Hotel)
            .WithMany(h => h.Rooms)
            .HasForeignKey(r => r.HotelId);
        modelBuilder
            .Entity<HotelModel>()
            .HasOne(h => h.City)
            .WithMany(c => c.Hotels)
            .HasForeignKey(h => h.CityId);

        // Seeder
        modelBuilder.SeedData();
    }
}
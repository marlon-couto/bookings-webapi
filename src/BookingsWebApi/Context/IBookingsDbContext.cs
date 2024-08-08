using BookingsWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Context;

public interface IBookingsDbContext
{
    DbSet<BookingModel> Bookings { get; set; }
    DbSet<CityModel> Cities { get; set; }
    DbSet<HotelModel> Hotels { get; set; }
    DbSet<RoomModel> Rooms { get; set; }
    DbSet<UserModel> Users { get; set; }
    Task<int> SaveChangesAsync();
}
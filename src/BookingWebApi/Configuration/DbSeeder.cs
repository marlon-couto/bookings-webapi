using BookingWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingWebApi.Data
{
    public static class DbSeeder
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Email = "user1@mail.com",
                    Name = "User 1",
                    Password = "Pass1",
                    Role = "Client",
                    UserId = "1"
                },
                new User
                {
                    Email = "user2@mail.com",
                    Name = "User 2",
                    Password = "Pass2",
                    Role = "Client",
                    UserId = "2"
                },
                new User
                {
                    Email = "user3@mail.com",
                    Name = "User 3",
                    Password = "Pass3",
                    Role = "Admin",
                    UserId = "3"
                }
            );

            modelBuilder.Entity<City>().HasData(
                new City
                {
                    CityId = "1",
                    Name = "City 1",
                    State = "State 1"
                },
                new City
                {
                    CityId = "2",
                    Name = "City 2",
                    State = "State 2"
                }
            );

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Address = "Address 1",
                    CityId = "1",
                    HotelId = "1",
                    Name = "Hotel 1"
                },
                new Hotel
                {
                    Address = "Address 2",
                    CityId = "2",
                    HotelId = "2",
                    Name = "Hotel 2"
                }
            );

            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Capacity = 2,
                    HotelId = "1",
                    Image = "Image 1",
                    Name = "Room 1",
                    RoomId = "1"
                },
                new Room
                {
                    Capacity = 1,
                    HotelId = "1",
                    Image = "Image 2",
                    Name = "Room 2",
                    RoomId = "2"
                },
                new Room
                {
                    Capacity = 3,
                    HotelId = "1",
                    Image = "Image 3",
                    Name = "Room 3",
                    RoomId = "3"
                },
                new Room
                {
                    Capacity = 2,
                    HotelId = "2",
                    Image = "Image 4",
                    Name = "Room 4",
                    RoomId = "4"
                },
                new Room
                {
                    Capacity = 1,
                    HotelId = "2",
                    Image = "Image 5",
                    Name = "Room 5",
                    RoomId = "5"
                },
                new Room
                {
                    Capacity = 3,
                    HotelId = "2",
                    Image = "Image 6",
                    Name = "Room 6",
                    RoomId = "6"
                }
            );

            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    BookingId = "1",
                    CheckIn = new DateTime(2023, 11, 7),
                    CheckOut = new DateTime(2023, 11, 8),
                    GuestQuantity = 1,
                    RoomId = "1",
                    UserId = "1"
                },
                new Booking
                {
                    BookingId = "2",
                    CheckIn = new DateTime(2023, 11, 7),
                    CheckOut = new DateTime(2023, 11, 8),
                    GuestQuantity = 1,
                    RoomId = "2",
                    UserId = "1"
                },
                new Booking
                {
                    BookingId = "3",
                    CheckIn = new DateTime(2023, 11, 7),
                    CheckOut = new DateTime(2023, 11, 8),
                    GuestQuantity = 1,
                    RoomId = "3",
                    UserId = "2"
                },
                new Booking
                {
                    BookingId = "4",
                    CheckIn = new DateTime(2023, 11, 7),
                    CheckOut = new DateTime(2023, 11, 8),
                    GuestQuantity = 1,
                    RoomId = "4",
                    UserId = "2"
                }
            );
        }
    }
}

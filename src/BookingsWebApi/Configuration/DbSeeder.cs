using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Configuration;

public static class DbSeeder
{
    public static void SeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<User>()
            .HasData(
                new User
                {
                    Email = "user1@mail.com",
                    Name = "User 1",
                    Password = "Of5n7kc14AwibmiFXk5ZnyDbioxlCiqxcZr7ayc/ad4=", // Pass@pass1
                    Role = "Client",
                    Id = "1",
                    Salt = "RDsG9pM5F0tj6ZIU2UogSA=="
                },
                new User
                {
                    Email = "user2@mail.com",
                    Name = "User 2",
                    Password = "7eyPpIQlkJGpp/lfFJuSeJ3IZbo5DznSu/C1VG7JO9I=", // Pass@pass2
                    Role = "Client",
                    Id = "2",
                    Salt = "Gqude0JZ38YVvz/RUTeZ9w=="
                },
                new User
                {
                    Email = "user3@mail.com",
                    Name = "User 3",
                    Password = "sT+OPanSmual937atM35GfT2Xp1w5yPXpTBA4U8JdEo=", // Pass@pass3
                    Role = "Admin",
                    Id = "3",
                    Salt = "pWIyxOGXD5363rNbX0ASfg=="
                }
            );

        modelBuilder
            .Entity<City>()
            .HasData(
                new City { Id = "1", Name = "City 1", State = "State 1" },
                new City { Id = "2", Name = "City 2", State = "State 2" }
            );

        modelBuilder
            .Entity<Hotel>()
            .HasData(
                new Hotel { Address = "Address 1", CityId = "1", Id = "1", Name = "Hotel 1" },
                new Hotel { Address = "Address 2", CityId = "2", Id = "2", Name = "Hotel 2" }
            );

        modelBuilder
            .Entity<Room>()
            .HasData(
                new Room
                {
                    Capacity = 2,
                    HotelId = "1",
                    Image = "Image 1",
                    Name = "Room 1",
                    Id = "1"
                },
                new Room
                {
                    Capacity = 1,
                    HotelId = "1",
                    Image = "Image 2",
                    Name = "Room 2",
                    Id = "2"
                },
                new Room
                {
                    Capacity = 3,
                    HotelId = "1",
                    Image = "Image 3",
                    Name = "Room 3",
                    Id = "3"
                },
                new Room
                {
                    Capacity = 2,
                    HotelId = "2",
                    Image = "Image 4",
                    Name = "Room 4",
                    Id = "4"
                },
                new Room
                {
                    Capacity = 1,
                    HotelId = "2",
                    Image = "Image 5",
                    Name = "Room 5",
                    Id = "5"
                },
                new Room
                {
                    Capacity = 3,
                    HotelId = "2",
                    Image = "Image 6",
                    Name = "Room 6",
                    Id = "6"
                }
            );

        modelBuilder
            .Entity<Booking>()
            .HasData(
                new Booking
                {
                    Id = "1",
                    CheckIn = new DateTime(2023, 11, 7),
                    CheckOut = new DateTime(2023, 11, 8),
                    GuestQuantity = 1,
                    RoomId = "1",
                    UserId = "1"
                },
                new Booking
                {
                    Id = "2",
                    CheckIn = new DateTime(2023, 11, 7),
                    CheckOut = new DateTime(2023, 11, 8),
                    GuestQuantity = 1,
                    RoomId = "2",
                    UserId = "1"
                },
                new Booking
                {
                    Id = "3",
                    CheckIn = new DateTime(2023, 11, 7),
                    CheckOut = new DateTime(2023, 11, 8),
                    GuestQuantity = 1,
                    RoomId = "3",
                    UserId = "2"
                },
                new Booking
                {
                    Id = "4",
                    CheckIn = new DateTime(2023, 11, 7),
                    CheckOut = new DateTime(2023, 11, 8),
                    GuestQuantity = 1,
                    RoomId = "4",
                    UserId = "2"
                }
            );
    }
}
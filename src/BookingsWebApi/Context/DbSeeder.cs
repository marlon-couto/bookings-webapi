using BookingsWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Context;

public static class DbSeeder
{
    public static void SeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<UserModel>()
            .HasData(
                new UserModel
                {
                    Email = "user1@mail.com",
                    Name = "User 1",
                    Password = "Of5n7kc14AwibmiFXk5ZnyDbioxlCiqxcZr7ayc/ad4=", // Pass@pass1
                    Role = "Client",
                    Id = Guid.Parse("05a347ae-1367-4e3b-b805-def7e8d901a6"),
                    Salt = "RDsG9pM5F0tj6ZIU2UogSA==",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                },
                new UserModel
                {
                    Email = "user2@mail.com",
                    Name = "User 2",
                    Password = "7eyPpIQlkJGpp/lfFJuSeJ3IZbo5DznSu/C1VG7JO9I=", // Pass@pass2
                    Role = "Client",
                    Id = Guid.Parse("498435d1-be9c-4f7f-8832-b3b043915c76"),
                    Salt = "Gqude0JZ38YVvz/RUTeZ9w==",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                },
                new UserModel
                {
                    Email = "user3@mail.com",
                    Name = "User 3",
                    Password = "sT+OPanSmual937atM35GfT2Xp1w5yPXpTBA4U8JdEo=", // Pass@pass3
                    Role = "Admin",
                    Id = Guid.Parse("6eff8e4c-a440-44ef-a001-6a4f4fa5f102"),
                    Salt = "pWIyxOGXD5363rNbX0ASfg==",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                }
            );
        modelBuilder
            .Entity<CityModel>()
            .HasData(
                new CityModel
                {
                    Id = Guid.Parse("ca4c3b99-c156-40b9-b3e2-9616d5cf05d2"),
                    Name = "City 1",
                    State = "State 1",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                },
                new CityModel
                {
                    Id = Guid.Parse("1e1ca2ff-eadb-45fd-a9c0-49384900f40b"),
                    Name = "City 2",
                    State = "State 2",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                }
            );
        modelBuilder
            .Entity<HotelModel>()
            .HasData(
                new HotelModel
                {
                    Address = "Address 1",
                    CityId = Guid.Parse("ca4c3b99-c156-40b9-b3e2-9616d5cf05d2"),
                    Id = Guid.Parse("b86aeb7a-a1a0-49ff-b9cb-5ff6ec7f7302"),
                    Name = "Hotel 1",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                },
                new HotelModel
                {
                    Address = "Address 2",
                    CityId = Guid.Parse("1e1ca2ff-eadb-45fd-a9c0-49384900f40b"),
                    Id = Guid.Parse("8c2ef74e-6c01-432d-b349-7867d112ba62"),
                    Name = "Hotel 2",
                    CreatedAt = DateTime.Now.ToUniversalTime()
                }
            );
        modelBuilder
            .Entity<RoomModel>()
            .HasData(
                new RoomModel
                {
                    Capacity = 2,
                    HotelId = Guid.Parse("b86aeb7a-a1a0-49ff-b9cb-5ff6ec7f7302"),
                    Image = "Image 1",
                    Name = "Room 1",
                    Id = Guid.Parse("cdd0c01b-d226-4c98-93ac-1aa199b55d3c"),
                    CreatedAt = DateTime.Now.ToUniversalTime()
                },
                new RoomModel
                {
                    Capacity = 1,
                    HotelId = Guid.Parse("b86aeb7a-a1a0-49ff-b9cb-5ff6ec7f7302"),
                    Image = "Image 2",
                    Name = "Room 2",
                    Id = Guid.Parse("7fd47a35-252b-4c1d-85cd-ba11629ade88"),
                    CreatedAt = DateTime.Now.ToUniversalTime()
                },
                new RoomModel
                {
                    Capacity = 3,
                    HotelId = Guid.Parse("b86aeb7a-a1a0-49ff-b9cb-5ff6ec7f7302"),
                    Image = "Image 3",
                    Name = "Room 3",
                    Id = Guid.Parse("7da42056-e3bd-4aae-8831-db637bb0a41e"),
                    CreatedAt = DateTime.Now.ToUniversalTime()
                },
                new RoomModel
                {
                    Capacity = 2,
                    HotelId = Guid.Parse("8c2ef74e-6c01-432d-b349-7867d112ba62"),
                    Image = "Image 4",
                    Name = "Room 4",
                    Id = Guid.Parse("5a568cf4-7b13-4e22-8779-1fffb3e13d50"),
                    CreatedAt = DateTime.Now.ToUniversalTime()
                },
                new RoomModel
                {
                    Capacity = 1,
                    HotelId = Guid.Parse("8c2ef74e-6c01-432d-b349-7867d112ba62"),
                    Image = "Image 5",
                    Name = "Room 5",
                    Id = Guid.Parse("d7df68b1-b57e-4626-8f91-9c9b9afd16f8"),
                    CreatedAt = DateTime.Now.ToUniversalTime()
                },
                new RoomModel
                {
                    Capacity = 3,
                    HotelId = Guid.Parse("8c2ef74e-6c01-432d-b349-7867d112ba62"),
                    Image = "Image 6",
                    Name = "Room 6",
                    Id = Guid.Parse("9aba51ec-a386-4697-8d0f-9487ae7d5516"),
                    CreatedAt = DateTime.Now.ToUniversalTime()
                }
            );
        modelBuilder
            .Entity<BookingModel>()
            .HasData(
                new BookingModel
                {
                    Id = Guid.Parse("5c75464d-b6cc-4f5c-927b-e324935dc0c1"),
                    CheckIn = DateTime.Now.ToUniversalTime(),
                    CheckOut = DateTime.Now.AddDays(1).ToUniversalTime(),
                    GuestQuantity = 1,
                    RoomId = Guid.Parse("cdd0c01b-d226-4c98-93ac-1aa199b55d3c"),
                    UserId = Guid.Parse("05a347ae-1367-4e3b-b805-def7e8d901a6"),
                    CreatedAt = DateTime.Now.ToUniversalTime()
                },
                new BookingModel
                {
                    Id = Guid.Parse("8671b4af-0a11-45a6-8a23-a32f5890bbf9"),
                    CheckIn = DateTime.Now.ToUniversalTime(),
                    CheckOut = DateTime.Now.AddDays(1).ToUniversalTime(),
                    GuestQuantity = 1,
                    RoomId = Guid.Parse("5a568cf4-7b13-4e22-8779-1fffb3e13d50"),
                    UserId = Guid.Parse("05a347ae-1367-4e3b-b805-def7e8d901a6"),
                    CreatedAt = DateTime.Now.ToUniversalTime()
                },
                new BookingModel
                {
                    Id = Guid.Parse("0300ec2e-bb50-4542-8aa2-167ea2721dc6"),
                    CheckIn = DateTime.Now.ToUniversalTime(),
                    CheckOut = DateTime.Now.AddDays(1).ToUniversalTime(),
                    GuestQuantity = 1,
                    RoomId = Guid.Parse("7fd47a35-252b-4c1d-85cd-ba11629ade88"),
                    UserId = Guid.Parse("498435d1-be9c-4f7f-8832-b3b043915c76"),
                    CreatedAt = DateTime.Now.ToUniversalTime()
                },
                new BookingModel
                {
                    Id = Guid.Parse("ed87365b-ff61-48f3-a7d5-b263a352c381"),
                    CheckIn = DateTime.Now.ToUniversalTime(),
                    CheckOut = DateTime.Now.AddDays(1).ToUniversalTime(),
                    GuestQuantity = 1,
                    RoomId = Guid.Parse("d7df68b1-b57e-4626-8f91-9c9b9afd16f8"),
                    UserId = Guid.Parse("498435d1-be9c-4f7f-8832-b3b043915c76"),
                    CreatedAt = DateTime.Now.ToUniversalTime()
                }
            );
    }
}

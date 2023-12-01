using System;

using Bogus;

using BookingsWebApi.Models;

namespace BookingsWebApi.Test.Builders;

public class BookingBuilder
{
    private readonly DateTime _checkIn;
    private readonly DateTime _checkOut;
    private readonly int _guestQuantity;
    private readonly string _id;
    private readonly Room _room;
    private User _user;

    private BookingBuilder()
    {
        Faker faker = new();
        _user = UserBuilder.New().Build();
        _guestQuantity = faker.Random.Int();
        _checkIn = DateTime.Now;
        _checkOut = DateTime.Now.AddDays(1);
        _room = RoomBuilder.New().Build();
        _id = faker.Random.Guid().ToString();
    }

    public static BookingBuilder New()
    {
        return new BookingBuilder();
    }

    public BookingBuilder WithUser(User user)
    {
        _user = user;
        return this;
    }

    public Booking Build()
    {
        return new Booking
        {
            CheckIn = _checkIn,
            CheckOut = _checkOut,
            GuestQuantity = _guestQuantity,
            Id = _id,
            Room = _room,
            User = _user,
            RoomId = _room.Id,
            UserId = _user.Id
        };
    }
}
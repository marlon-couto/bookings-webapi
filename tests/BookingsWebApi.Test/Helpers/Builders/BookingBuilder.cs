using System;
using System.Globalization;

using Bogus;

using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

namespace BookingsWebApi.Test.Helpers.Builders;

public class BookingBuilder
{
    private readonly DateTime _checkIn;
    private readonly DateTime _checkOut;
    private readonly string _id;
    private readonly RoomModel _room;
    private string _checkInStr;
    private string _checkOutStr;
    private int _guestQuantity;
    private string _roomId;
    private UserModel _user;

    private BookingBuilder()
    {
        Faker faker = new();
        _user = UserBuilder.New().Build();
        _guestQuantity = faker.Random.Int(1);
        _checkIn = DateTime.Now;
        _checkInStr = _checkIn.ToString(CultureInfo.InvariantCulture);
        _checkOut = DateTime.Now.AddDays(1);
        _checkOutStr = _checkOut.ToString(CultureInfo.InvariantCulture);
        _room = RoomBuilder.New().Build();
        _roomId = _room.Id;
        _id = faker.Random.Guid().ToString();
    }

    public static BookingBuilder New()
    {
        return new BookingBuilder();
    }

    public BookingBuilder WithUser(UserModel user)
    {
        _user = user;
        return this;
    }

    public BookingBuilder WithCheckIn(string checkInStr)
    {
        _checkInStr = checkInStr;
        return this;
    }

    public BookingBuilder WithCheckOut(string checkOutStr)
    {
        _checkOutStr = checkOutStr;
        return this;
    }

    public BookingBuilder WithGuestQuantity(int guestQuantity)
    {
        _guestQuantity = guestQuantity;
        return this;
    }

    public BookingBuilder WithRoomId(string roomId)
    {
        _roomId = roomId;
        return this;
    }

    public BookingModel Build()
    {
        return new BookingModel
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

    public BookingInsertDto BuildAsInsertDto()
    {
        return new BookingInsertDto
        {
            CheckIn = _checkInStr, CheckOut = _checkOutStr, GuestQuantity = _guestQuantity, RoomId = _roomId
        };
    }
}
using Bogus;

using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

namespace BookingsWebApi.Test.Builders;

public class RoomBuilder
{
    private readonly string _id;
    private int _capacity;
    private Hotel _hotel;
    private string _hotelId;
    private string _image;
    private string _name;

    private RoomBuilder()
    {
        Faker faker = new();
        _id = faker.Random.Guid().ToString();
        _image = faker.Image.PicsumUrl();
        _name = faker.Lorem.Sentence(1, 3);
        _hotel = HotelBuilder.New().Build();
        _hotelId = _hotel.Id;
        _capacity = faker.Random.Int(1);
    }

    public static RoomBuilder New()
    {
        return new RoomBuilder();
    }

    public RoomBuilder WithHotel(Hotel hotel)
    {
        _hotel = hotel;
        return this;
    }

    public RoomBuilder WithCapacity(int capacity)
    {
        _capacity = capacity;
        return this;
    }

    public RoomBuilder WithHotelId(string hotelId)
    {
        _hotelId = hotelId;
        return this;
    }

    public RoomBuilder WithImage(string image)
    {
        _image = image;
        return this;
    }

    public RoomBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public Room Build()
    {
        return new Room
        {
            Id = _id,
            Name = _name,
            Capacity = _capacity,
            HotelId = _hotel.Id,
            Image = _image,
            Hotel = _hotel
        };
    }

    public RoomInsertDto BuildAsInsertDto()
    {
        return new RoomInsertDto { Capacity = _capacity, HotelId = _hotelId, Image = _image, Name = _name };
    }
}
using Bogus;

using BookingsWebApi.Models;

namespace BookingsWebApi.Test.Builders;

public class RoomBuilder
{
    private readonly int _capacity;
    private readonly string _id;
    private readonly string _image;
    private readonly string _name;
    private Hotel _hotel;

    private RoomBuilder()
    {
        Faker faker = new();
        _id = faker.Random.Guid().ToString();
        _image = faker.Image.PicsumUrl();
        _name = faker.Lorem.Sentence();
        _hotel = HotelBuilder.New().Build();
        _capacity = faker.Random.Int();
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
}
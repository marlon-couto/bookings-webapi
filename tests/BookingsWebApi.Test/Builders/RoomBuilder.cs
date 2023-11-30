using Bogus;

using BookingsWebApi.Models;

namespace BookingsWebApi.Test.Builders;

public class RoomBuilder
{
    private readonly int _capacity;
    private readonly Hotel _hotel;
    private readonly string _hotelId;
    private readonly string _id;
    private readonly string _image;
    private readonly string _name;

    private RoomBuilder()
    {
        Faker faker = new();
        _id = faker.Random.Guid().ToString();
        _image = faker.Image.PicsumUrl();
        _name = faker.Lorem.Sentence();
        _hotel = HotelBuilder.New().Build();
        _hotelId = _hotel.Id;
        _capacity = faker.Random.Int();
    }

    public static RoomBuilder New()
    {
        return new RoomBuilder();
    }

    public Room Build()
    {
        return new Room
        {
            Id = _id,
            Name = _name,
            Capacity = _capacity,
            HotelId = _hotelId,
            Image = _image,
            Hotel = _hotel
        };
    }
}
using Bogus;

using BookingsWebApi.Models;

namespace BookingsWebApi.Test.Builders;

public class HotelBuilder
{
    private readonly string _address;
    private readonly City _city;
    private readonly string _cityId;
    private readonly string _id;
    private readonly string _name;

    private HotelBuilder()
    {
        Faker faker = new();
        _city = CityBuilder.New().Build();
        _cityId = _city.Id;
        _name = faker.Lorem.Sentence();
        _id = faker.Random.Guid().ToString();
        _address = faker.Address.FullAddress();
    }

    public static HotelBuilder New()
    {
        return new HotelBuilder();
    }

    public Hotel Build()
    {
        return new Hotel
        {
            Id = _id,
            Name = _name,
            Address = _address,
            CityId = _cityId,
            City = _city
        };
    }
}
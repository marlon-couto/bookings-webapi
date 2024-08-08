using Bogus;
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

namespace BookingsWebApi.Test.Helpers.Builders;

public class HotelBuilder
{
    private readonly CityModel _city;
    private readonly string _id;
    private string _address;
    private string _cityId;
    private string _name;

    private HotelBuilder()
    {
        Faker faker = new();
        _city = CityBuilder.New().Build();
        _cityId = _city.Id;
        _name = faker.Lorem.Sentence(1, 3);
        _id = faker.Random.Guid().ToString();
        _address = faker.Address.FullAddress();
    }

    public static HotelBuilder New()
    {
        return new HotelBuilder();
    }

    public HotelBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public HotelBuilder WithAddress(string address)
    {
        _address = address;
        return this;
    }

    public HotelBuilder WithCityId(string cityId)
    {
        _cityId = cityId;
        return this;
    }

    public HotelModel Build()
    {
        return new HotelModel
        {
            Id = _id,
            Name = _name,
            Address = _address,
            CityId = _city.Id,
            City = _city
        };
    }

    public HotelInsertDto BuildAsInsertDto()
    {
        return new HotelInsertDto { Address = _address, CityId = _cityId, Name = _name };
    }
}
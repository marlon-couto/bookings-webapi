using Bogus;
using BookingsWebApi.Models;

namespace BookingsWebApi.Test.Builders;

public class CityBuilder
{
    private readonly string _id;
    private readonly string _name;
    private readonly string _state;

    private CityBuilder()
    {
        Faker faker = new();
        _name = faker.Address.City();
        _state = faker.Address.State();
        _id = faker.Random.Guid().ToString();
    }

    public static CityBuilder New()
    {
        return new CityBuilder();
    }

    public City Build()
    {
        return new City
        {
            Id = _id,
            Name = _name,
            State = _state
        };
    }
}

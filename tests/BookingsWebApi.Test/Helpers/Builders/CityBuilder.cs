using System;
using Bogus;
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

namespace BookingsWebApi.Test.Helpers.Builders;

public class CityBuilder
{
    private readonly Guid _id;
    private string _name;
    private string _state;

    private CityBuilder()
    {
        Faker faker = new();
        _name = faker.Address.City();
        _state = faker.Address.State();
        _id = faker.Random.Guid();
    }

    public static CityBuilder New()
    {
        return new CityBuilder();
    }

    public CityBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CityBuilder WithState(string state)
    {
        _state = state;
        return this;
    }

    public CityModel Build()
    {
        return new CityModel { Id = _id, Name = _name, State = _state };
    }

    public CityInsertDto BuildAsInsertDto()
    {
        return new CityInsertDto { Name = _name, State = _state };
    }
}
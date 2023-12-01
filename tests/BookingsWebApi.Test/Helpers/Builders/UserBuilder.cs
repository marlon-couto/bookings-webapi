using Bogus;

using BookingsWebApi.Models;

namespace BookingsWebApi.Test.Helpers.Builders;

public class UserBuilder
{
    private readonly string _email;
    private readonly string _id;
    private readonly string _name;
    private readonly string _password;
    private readonly string _role;

    private UserBuilder()
    {
        Faker faker = new();
        _email = faker.Internet.Email();
        _password = faker.Internet.Password();
        _role = "Client";
        _name = faker.Name.FirstName();
        _id = faker.Random.Guid().ToString();
    }

    public static UserBuilder New()
    {
        return new UserBuilder();
    }

    public User Build()
    {
        return new User
        {
            Role = _role,
            Password = _password,
            Email = _email,
            Id = _id,
            Name = _name
        };
    }
}
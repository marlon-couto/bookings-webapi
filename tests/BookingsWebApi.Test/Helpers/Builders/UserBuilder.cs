using Bogus;

using BookingsWebApi.DTOs;
using BookingsWebApi.Helpers;
using BookingsWebApi.Models;

namespace BookingsWebApi.Test.Helpers.Builders;

public class UserBuilder
{
    private readonly string _id;
    private readonly string _role;
    private string _email;
    private string _name;
    private string _password;
    private string _passwordHashed;
    private string _salt;

    private UserBuilder()
    {
        Faker faker = new();
        _email = faker.Internet.Email();
        _password = faker.Internet.Password(prefix: "Pass@12", length: 10);
        _passwordHashed = HashPassword.EncryptPassword(_password, out string salt);
        _salt = salt;
        _role = "Client";
        _name = faker.Name.FirstName();
        _id = faker.Random.Guid().ToString();
    }

    public static UserBuilder New()
    {
        return new UserBuilder();
    }

    public UserBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public UserBuilder WithPassword(string password)
    {
        _password = password;
        _passwordHashed = HashPassword.EncryptPassword(password, out string salt);
        _salt = salt;
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public User Build()
    {
        return new User
        {
            Role = _role,
            Password = _passwordHashed,
            Email = _email,
            Id = _id,
            Name = _name,
            Salt = _salt
        };
    }

    public UserInsertDto BuildAsInsertDto()
    {
        return new UserInsertDto { Email = _email, Name = _name, Password = _password };
    }

    public LoginInsertDto BuildAsLoginDto()
    {
        return new LoginInsertDto { Email = _email, Password = _password };
    }
}
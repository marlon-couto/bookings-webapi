using BookingsWebApi.Models;

namespace BookingsWebApi.Services
{
    public interface ITokenGenerator
    {
        public string Generate(User user);
    }
}

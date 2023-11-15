using BookingsWebApi.Dtos;
using BookingsWebApi.Repositories;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly IValidator<UserInsertDto> _validator;
        public UserController(IUserRepository repository, IValidator<UserInsertDto> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            List<UserDto> allUsers = await _repository.GetAllUsers();
            return Ok(allUsers);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] UserInsertDto inputData)
        {
            var validationResult = await _validator.ValidateAsync(inputData);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Message = validationResult.Errors[0].ErrorMessage });
            }

            bool emailExists = await _repository.EmailExists(inputData.Email);
            if (emailExists)
            {
                return Conflict(new { Message = "The email provided is already registered" });
            }

            UserDto createdUser = await _repository.AddUser(inputData);
            return Created("/api/user", createdUser);
        }
    }
}

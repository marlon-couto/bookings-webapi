using BookingsWebApi.Dtos;
using BookingsWebApi.Models;
using BookingsWebApi.Repositories;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : Controller
    {
        private readonly IHotelRepository _repository;
        private readonly IValidator<HotelInsertDto> _validator;
        public HotelController(IHotelRepository repository, IValidator<HotelInsertDto> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            List<HotelDto> allHotels = await _repository.GetAllHotels();
            return Ok(allHotels);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] HotelInsertDto inputData)
        {
            var validationResult = await _validator.ValidateAsync(inputData);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Message = validationResult.Errors[0].ErrorMessage });
            }

            City? cityFound = await _repository.GetCityById(inputData.CityId);
            if (cityFound is null)
            {
                return BadRequest(new { Message = "The city with the id provided does not exist" });
            }

            HotelDto createdHotel = await _repository.AddHotel(inputData, cityFound);
            return Created("/api/hotel", createdHotel);
        }
    }
}

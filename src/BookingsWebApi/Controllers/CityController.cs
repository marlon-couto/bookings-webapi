using BookingsWebApi.Dtos;
using BookingsWebApi.Models;
using BookingsWebApi.Repositories;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : Controller
    {
        private readonly ICityRepository _repository;
        private readonly IValidator<CityInsertDto> _validator;
        public CityController(ICityRepository repository, IValidator<CityInsertDto> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            List<CityDto> allCities = await _repository.GetAllCities();
            return Ok(allCities);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(CityInsertDto cityInsert)
        {
            var validationResult = await _validator.ValidateAsync(cityInsert);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Message = validationResult.Errors[0].ErrorMessage });
            }

            CityDto createdCity = await _repository.AddCity(cityInsert);
            return Created("/api/city", createdCity);
        }

        [HttpPut("{cityId}")]
        public async Task<IActionResult> PutAsync([FromBody] CityInsertDto cityInsert, string cityId)
        {
            var validationResult = await _validator.ValidateAsync(cityInsert);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Message = validationResult.Errors[0].ErrorMessage });
            }

            City? cityFound = await _repository.GetCityById(cityId);
            if (cityFound is null)
            {
                return NotFound(new { Message = "The city with the id provided does not exist" });
            }

            CityDto updatedCity = _repository.UpdateCity(cityInsert, cityFound);
            return Ok(updatedCity);
        }
    }
}

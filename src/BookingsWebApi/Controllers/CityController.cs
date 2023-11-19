using BookingsWebApi.Dtos;
using BookingsWebApi.Models;
using BookingsWebApi.Repositories;

using FluentValidation;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Policy = "Admin")]
    public class CityController : Controller
    {
        private readonly ICityRepository _repository;
        private readonly IValidator<CityInsertDto> _validator;
        public CityController(ICityRepository repository, IValidator<CityInsertDto> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        /// <summary>
        /// Retrieves all cities information.
        /// </summary>
        /// <returns>A JSON response representing the result of the operation</returns>
        /// <response code="200">Returns 200 and the city data.</response>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync()
        {
            List<CityDto> allCities = await _repository.GetAllCities();
            return Ok(new { Data = allCities, Result = "Success" });
        }

        /// <summary>
        /// Creates a new city based on the provided data.
        /// </summary>
        /// <param name="inputData">The data for creating a new city.</param>
        /// <returns>A JSON response representing the result of the operation.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /City
        ///     {
        ///         "name": "New City",
        ///         "state": "State 1"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns 201 and the newly created city data.</response>
        /// <response code="401">If the user is unauthorized, returns 401.</response>
        /// <response code="400">If the input data is invalid, returns 400 and an error message.</response>
        [HttpPost]
        public async Task<IActionResult> PostAsync(CityInsertDto inputData)
        {
            try
            {
                await ValidateInputData(inputData);

                CityDto createdCity = await _repository.AddCity(inputData);
                return Created("/api/city", new { Data = createdCity, Result = "Success" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { ex.Message, Result = "Error" });
            }
        }

        /// <summary>
        /// Updates the city with the given ID based on the provided data.
        /// </summary>
        /// <param name="inputData">The data for updating the city retrieved.</param>
        /// <param name="id">The ID of the city to update.</param>
        /// <returns>A JSON response representing the result of the operation.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /City/1
        ///     {
        ///         "name": "New City",
        ///         "state": "State 1"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns 201 and the updated city data.</response>
        /// <response code="401">If the user is unauthorized, returns 401.</response>
        /// <response code="400">If the input data is invalid, returns 400 and an error message.</response>
        /// <response code="404">If a city with the provided ID not exists, returns 404 and an error message.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromBody] CityInsertDto inputData, string id)
        {
            try
            {
                await ValidateInputData(inputData);

                City cityFound = await _repository.GetCityById(id);

                CityDto updatedCity = _repository.UpdateCity(cityFound, inputData);
                return Ok(new { Data = updatedCity, Result = "Success" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { ex.Message, Result = "Error" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message, Result = "Error" });
            }
        }

        /// <summary>
        /// Deletes a city with the given ID.
        /// </summary>
        /// <param name="id">The ID of the city to delete.</param>
        /// <returns>A status code 204 and no content.</returns>
        /// <response code="204">Returns 204 with no content.</response>
        /// <response code="401">If the user is unauthorized, returns 401.</response>
        /// <response code="404">If the city is not found, returns 404 and an error message.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                City cityFound = await _repository.GetCityById(id);
                _repository.DeleteCity(cityFound);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message, Result = "Error" });
            }
        }

        private async Task ValidateInputData(CityInsertDto inputData)
        {
            var validationResult = await _validator.ValidateAsync(inputData);
            if (!validationResult.IsValid)
            {
                List<string> errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ArgumentException(string.Join(" ", errorMessages));
            }
        }
    }
}

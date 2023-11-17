using BookingsWebApi.Dtos;
using BookingsWebApi.Models;
using BookingsWebApi.Repositories;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomRepository _repository;
        private readonly IValidator<RoomInsertDto> _validator;
        public RoomController(IRoomRepository repository, IValidator<RoomInsertDto> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                Room roomFound = await _repository.GetRoomById(id);
                _repository.DeleteRoom(roomFound);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message, Result = "Error" });
            }
        }

        [HttpGet("{hotelId}")]
        public async Task<IActionResult> GetAsync(string hotelId)
        {
            try
            {
                await _repository.GetHotelById(hotelId);

                List<RoomDto> hotelRooms = await _repository.GetHotelRooms(hotelId);
                return Ok(new { Data = hotelRooms, Result = "Success" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message, Result = "Error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] RoomInsertDto inputData)
        {
            try
            {
                await ValidateInputData(inputData);

                Hotel hotelFound = await _repository.GetHotelById(inputData.HotelId);

                RoomDto createdRoom = await _repository.AddRoom(inputData, hotelFound);
                return Created($"/api/room/{inputData.HotelId}", new { Data = createdRoom, Result = "Success" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { ex.Message, Result = "Error" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message, Result = "Error" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { ex.Message, Result = "Error" });
            }
        }

        private async Task ValidateInputData(RoomInsertDto inputData)
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

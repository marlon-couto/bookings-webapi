using BookingsWebApi.Dtos;
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

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] RoomInsertDto inputData)
        {
            return Created("", null);
        }
    }
}

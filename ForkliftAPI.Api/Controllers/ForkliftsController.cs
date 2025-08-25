using ForkliftAPI.Application.DTOs;
using ForkliftAPI.Application.Interfaces;
using ForkliftAPI.Application.Services;
using ForkliftAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForkliftAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForkliftsController : ControllerBase
    {
        private readonly IForkliftService _service;

        public ForkliftsController(IForkliftService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ForkliftDto>>> Get()
        {
            var forkliftsDto = await _service.GetAllForkliftsAsync();
            return Ok(forkliftsDto);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromBody] List<ForkliftDto> forkliftsDto)
        {
            if (forkliftsDto == null || !forkliftsDto.Any())
                return BadRequest("No forklift data received.");

            var count = await _service.UploadForkliftsAsync(forkliftsDto);
            return Ok(new { message = $"Uploaded {count} forklifts." });
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearAllForklifts()
        {
            await _service.ClearAllForkliftsAsync();
            return NoContent();
        }

        [HttpGet("commands/{modelNumber}")]
        public async Task<ActionResult<List<ForkliftCommandDto>>> GetForkliftCommandById(string modelNumber)
        {
            var forkliftCommandDto = await _service.GetForkliftCommandByIdAsync(modelNumber);
            if (forkliftCommandDto == null)
                return NotFound();

            return Ok(forkliftCommandDto);
        }

        [HttpPost("command")]
        public async Task<bool> SubmitForkliftCommandAsync([FromBody] ForkliftCommandDto commandDto)
        {
            if (commandDto == null)
                return false;

            return await _service.SubmitForkliftCommandAsync(commandDto);
        }

    }
}

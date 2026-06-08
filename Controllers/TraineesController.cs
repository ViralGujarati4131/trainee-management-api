using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.DTOs;
using TraineeManagementApi.Service.Interface;

namespace TraineeManagement.Api.Controllers;

[ApiController]
// [Route("api/[controller]")]
public class TraineesController : ControllerBase
{
    private readonly ITraineeService traineeService;
    public TraineesController(ITraineeService service)
    {
        traineeService = service;
    }

    [HttpGet("api/trainee/{id}")]
    // [HttpGet("{id}")]
    public async Task<ActionResult<TraineeResponseDto>> GetTraineeById(int id)
    {
        var traineeById = await traineeService.GetTraineeByIdService(id);
        if (traineeById == null)
        {
            return NotFound(new { Message = $"Trainee with Id {id} not found" });
        }
        return Ok(traineeById);
    }
 
    [HttpPost("api/trainee")]
    // [HttpPost]
    public async Task<ActionResult<TraineeResponseDto>> CreateTrainee([FromBody] CreateTraineeDto trainee)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var t = await traineeService.CreateTraineeService(trainee);
        return CreatedAtAction(nameof(GetTraineeById),new {id = t.Id}, t);
    }

    [HttpPut("api/trainee/{id:int}")]
    // [HttpPut("{id}")]
    public async Task<ActionResult<TraineeResponseDto>> UpdateTraineeById(int id,[FromBody] UpdateTraineeDto updatedTrainee)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var trainee = await traineeService.UpdateTraineeService(id,updatedTrainee);
        if (trainee == null)
        {
            return NotFound(new { Message = $"Trainee with Id {id} not found" });
        }
        return Ok(trainee);
    }

    [HttpDelete("api/trainee/{id:int}")]
    // [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTraineeById(int id)
    {
        if (!await traineeService.DeleteTraineeByIdService(id))
        {
            return NotFound(new { Message = $"Trainee with Id {id} not found" });
        }
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpGet("api/trainee")]
    // [HttpGet]
    public async Task<ActionResult<TraineeResponseDto>> GetTrainee([FromQuery] string? search)
    {
        if (search == null)
        {
            var traineeAll = await traineeService.GetTraineeService();
            if (!traineeAll.Any())
            {
                return NotFound(new { Message = "No trainee found" });
            }
            return Ok(traineeAll);
        }
        var searchResult = await traineeService.SearchTraineesService(search);
        if (!searchResult.Any())
        {
            return NotFound(new { Message = $"No trainee found matching '{search}'" });
        }
        return Ok(searchResult);
    }

 }
    


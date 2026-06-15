using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.Utils.CustomException;
using TraineeManagementApi.Trainees.DTOs;
using TraineeManagementApi.Trainees.ServiceInterface;

namespace TraineeManagementApi.Trainees.Controller;

[ApiController]
[Route("api/trainee")]
[Authorize]
public class TraineesController : ControllerBase
{
    private readonly ITraineeService _traineeService;
    private readonly ILogger<TraineesController> _logger;

    public TraineesController(ITraineeService traineeService, ILogger<TraineesController> logger)
    {
        _traineeService = traineeService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TraineeResponseDto>> GetTraineeById(int id)
    {
        _logger.LogDebug("Invoking trainee service to retrieve profile for TraineeId: {TraineeId}", id);
        TraineeResponseDto trainee = await _traineeService.GetTraineeByIdAsync(id);
        return Ok(trainee);
    }

    [HttpPost]
    public async Task<ActionResult<TraineeResponseDto>> CreateTrainee([FromBody] TraineeCreateDto createTraineeDto)
    {
        _logger.LogDebug("Invoking trainee service to establish a new trainee registration");
        TraineeResponseDto createdTrainee = await _traineeService.CreateTraineeAsync(createTraineeDto);
        return CreatedAtAction(nameof(GetTraineeById), new { id = createdTrainee.Id }, createdTrainee);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TraineeResponseDto>> UpdateTraineeById(int id, [FromBody] TraineeUpdateDto updateTraineeDto)
    {
        _logger.LogDebug("Invoking trainee service to modify records for TraineeId: {TraineeId}", id);
        TraineeResponseDto updatedTrainee = await _traineeService.UpdateTraineeAsync(id, updateTraineeDto);
        return Ok(updatedTrainee);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTraineeById(int id)
    {
        _logger.LogDebug("Invoking trainee service to delete records for TraineeId: {TraineeId}", id);
        await _traineeService.DeleteTraineeByIdAsync(id);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TraineeResponseDto>>> GetTrainees([FromQuery] string? searchTrainee)
    {
        if (searchTrainee == null)
        {
            _logger.LogDebug("Invoking trainee service to fetch all trainees");
            IEnumerable<TraineeResponseDto> trainees = await _traineeService.GetTraineesAsync();
            return Ok(trainees);
        }
        _logger.LogDebug("Invoking trainee service to query profiles matching search criteria: {SearchTerm}", searchTrainee);
        IEnumerable<TraineeResponseDto> searchResults = await _traineeService.SearchTraineesAsync(searchTrainee);
        return Ok(searchResults);
    }

    [HttpGet("paginationSearch")]
    public async Task<ActionResult<TraineePaginationSearchDto>> PaginationSearchTrainee([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? name, [FromQuery] string? status)
    {
        if (pageNumber <= 0 || pageSize <= 0 || name == null || status == null)
        {
            _logger.LogWarning("Pagination request processing aborted due to missing or invalid filter arguments");
            throw new BadRequestException("All fields are require");
        }
        _logger.LogDebug("Invoking trainee service to generate paginated lookup - Name: {FilterName}, Status: {FilterStatus}, Page: {PageNumber}, Size: {PageSize}", name, status, pageNumber, pageSize);
        TraineePaginationSearchDto? pagedData = await _traineeService.GetPagedAndSearchedTraineesAsync(pageNumber, pageSize, name, status);
        return Ok(pagedData);
    }
}
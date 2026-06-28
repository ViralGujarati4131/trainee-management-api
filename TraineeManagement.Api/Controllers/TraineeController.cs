using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Data.TraineeDTO;
using TraineeManagement.Api.TraineeServiceInterface;
using TraineeManagement.Api.ResponsesBuilder;
using TraineeManagement.Api.Data.ConstRoute;
using TraineeManagement.Api.Data.Response;

namespace TraineeManagement.Api.TraineeController;

[ApiController]
[Route(CustomConstRoute.Trainees)]
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
    public async Task<ActionResult> GetTraineeById(int id,CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        if(id < 1)
        {
            _logger.LogWarning("Request failed validation. Invalid ID range. Id: {TraineeId}", id);
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }
        _logger.LogDebug("Invoking trainee service to retrieve profile for TraineeId: {TraineeId}", id);
        
        TraineeResponseDto? trainee = await _traineeService.GetTraineeByIdAsync(id,cancellationToken);
        
        _logger.LogInformation("State check: Fetch trainee by ID success. Id: {TraineeId}", id);
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataRetrivedSuccess,
            trainee
        );
    }

    [HttpPost]
    public async Task<ActionResult> CreateTrainee([FromBody] TraineeCreateDto createTraineeDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        _logger.LogDebug("Invoking trainee service to establish a new trainee registration");
        
        TraineeResponseDto createdTrainee = await _traineeService.CreateTraineeAsync(createTraineeDto);
        
        _logger.LogInformation("State check: Trainee creation success. Id: {TraineeId}", createdTrainee.Id);
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataInsertedSuccess,
            createdTrainee
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTraineeById(int id, [FromBody] TraineeUpdateDto updateTraineeDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        if(id < 1)
        {
            _logger.LogWarning("Request failed validation. Invalid ID range. Id: {TraineeId}", id);
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }
        _logger.LogDebug("Invoking trainee service to modify records for TraineeId: {TraineeId}", id);
        
        TraineeResponseDto updatedTrainee = await _traineeService.UpdateTraineeAsync(id, updateTraineeDto);
        
        _logger.LogInformation("State check: Trainee modification success. Id: {TraineeId}", id);
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataUpdatedSuccess,
            updatedTrainee
        );
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTraineeById(int id)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        if(id < 1)
        {
            _logger.LogWarning("Request failed validation. Invalid ID range. Id: {TraineeId}", id);
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }
        _logger.LogDebug("Invoking trainee service to delete records for TraineeId: {TraineeId}", id);
        
        await _traineeService.DeleteTraineeByIdAsync(id);
        
        _logger.LogInformation("State check: Trainee deletion success. Id: {TraineeId}", id);
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataDeletedNoContent
        );
    }

    [HttpGet]
    public async Task<ActionResult> GetTrainees([FromQuery] string? searchTrainee)
    {
        if (searchTrainee == null)
        {
            _logger.LogDebug("Invoking trainee service to fetch all trainees");

            IEnumerable<TraineeResponseDto> trainees = await _traineeService.GetTraineesAsync();

            _logger.LogInformation("State check: Bulk fetch trainees success.");
            return CustomResponseBuilder.CreateSuccessResponse(
                CustomResponse.DataRetrivedSuccess,
                trainees
            );
        }
        _logger.LogDebug("Invoking trainee service to query profiles matching search criteria: {SearchTerm}", searchTrainee);
        
        IEnumerable<TraineeResponseDto> searchResults = await _traineeService.SearchTraineesAsync(searchTrainee);
        
        _logger.LogInformation("State check: Trainee lookup matching search query completed.");
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataRetrivedSuccess,
            searchResults
        );
    }

    [HttpGet(CustomConstRoute.PaginationSearch)]
    public async Task<ActionResult> PaginationSearchTrainee([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string name, [FromQuery] string status)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Pagination request processing aborted due to missing or invalid filter arguments");

            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        if (pageNumber < 1 || pageSize < 1)
        {
            _logger.LogWarning("Pagination request processing aborted due to missing or invalid filter arguments");

            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }
        _logger.LogDebug("Invoking trainee service to generate paginated lookup - Name: {FilterName}, Status: {FilterStatus}, Page: {PageNumber}, Size: {PageSize}", name, status, pageNumber, pageSize);
        
        TraineePaginationSearchDto? pagedData = await _traineeService.GetPagedAndSearchedTraineesAsync(pageNumber, pageSize, name, status);
        
        _logger.LogInformation("State check: Paginated search results generated successfully.");
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataRetrivedSuccess,
            pagedData
        );
    }
}
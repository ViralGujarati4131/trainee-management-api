using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Data.TaskAssignmentDTO;
using TraineeManagement.Api.TaskAssignmentServiceInterface;
using TraineeManagement.Api.ResponsesBuilder;
using TraineeManagement.Api.Data.ConstRoute;
using TraineeManagement.Api.Data.Response;

namespace TraineeManagement.Api.TaskAssignmentController;

[ApiController]
[Route(CustomConstRoute.TaskAssignments)]
[Authorize]
public class TaskAssignmentsController : ControllerBase
{
    private readonly ITaskAssignmentService _taskAssignmentService;

    private readonly ILogger<TaskAssignmentsController> _logger;

    public TaskAssignmentsController(ITaskAssignmentService taskAssignmentService, ILogger<TaskAssignmentsController> logger)
    {
        _taskAssignmentService = taskAssignmentService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult> CreateTaskAssignment([FromBody] TaskAssignmentCreateDto createTaskAssignmentDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        _logger.LogDebug("Invoking task-assignment service to add a new task-assignment");

        TaskAssignmentResponseDto createdTaskAssignment = await _taskAssignmentService.CreateTaskAssignmentAsync(createTaskAssignmentDto);
        
        _logger.LogInformation("State check: Task assignment creation success. Id: {AssignmentId}", createdTaskAssignment.Id);
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataInsertedSuccess,
            createdTaskAssignment
        );
    }

    [HttpGet]
    public async Task<ActionResult> GetTaskAssignments()
    {
        _logger.LogDebug("Invoking task-assignment service to fetch all task-assignments");

        IEnumerable<TaskAssignmentResponseDto> taskAssignments = await _taskAssignmentService.GetTaskAssignmentsAsync();

        _logger.LogInformation("State check: Bulk fetch task assignments success.");
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataRetrivedSuccess,
            taskAssignments
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetTaskAssignmentById(int id)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        if(id < 1)
        {
            _logger.LogWarning("Request failed validation. Invalid ID range. Id: {AssignmentId}", id);
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }
        _logger.LogDebug("Invoking task-assignment service to retrieve assignments for AssignmentId: {AssignmentId}", id);

        TaskAssignmentResponseDto taskAssignment = await _taskAssignmentService.GetTaskAssignmentByIdAsync(id);

        _logger.LogInformation("State check: Fetch task assignment by ID success. Id: {AssignmentId}", id);
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataRetrivedSuccess,
            taskAssignment
        );
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult> UpdateTaskAssignmentById(int id, [FromBody] TaskAssignmentUpdateDto updateTaskAssignmentDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        if(id < 1)
        {
            _logger.LogWarning("Request failed validation. Invalid ID range. Id: {AssignmentId}", id);
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }
        _logger.LogDebug("Invoking task-assignment service to modify records for AssignmentId: {AssignmentId}", id);
        
        TaskAssignmentResponseDto updatedTaskAssignment = await _taskAssignmentService.UpdateTaskAssignmentAsync(id, updateTaskAssignmentDto);
        
        _logger.LogInformation("State check: Task assignment modification success. Id: {AssignmentId}", id);
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataUpdatedSuccess,
            updatedTaskAssignment
        );
    }
}
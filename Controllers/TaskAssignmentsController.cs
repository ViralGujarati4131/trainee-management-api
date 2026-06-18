using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.TaskAssignments.DTOs;
using TraineeManagementApi.TaskAssignments.ServiceInterface;
using TraineeManagementApi.ResponsesBuilder;
using TraineeManagementApi.Constants;

namespace TraineeManagementApi.TaskAssignments.Controller;

[ApiController]
[Route(AppConstants.Routes.TaskAssignments)]
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
        if(!ModelState.IsValid)
        {
            return ResponseBuilder.CreateResponse(StatusCodes.Status400BadRequest,AppConstants.Errors.ValidationFailed,ModelState);
        }
        _logger.LogDebug("Invoking task-assignment service to add a new task-assignment");

        TaskAssignmentResponseDto createdTaskAssignment = await _taskAssignmentService.CreateTaskAssignmentAsync(createTaskAssignmentDto);
        
        return ResponseBuilder.CreateResponseSuccess(StatusCodes.Status200OK,createdTaskAssignment);
    }

    [HttpGet]
    public async Task<ActionResult> GetTaskAssignments()
    {
        _logger.LogDebug("Invoking task-assignment service to fetch all task-assignments");

        IEnumerable<TaskAssignmentResponseDto> taskAssignments = await _taskAssignmentService.GetTaskAssignmentsAsync();

        return ResponseBuilder.CreateResponseSuccess(StatusCodes.Status200OK,taskAssignments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetTaskAssignmentById(int id)
    {
        if(!ModelState.IsValid || id < 1)
        {
            return ResponseBuilder.CreateResponse(StatusCodes.Status400BadRequest,AppConstants.Errors.ValidationFailed,ModelState);
        }
        _logger.LogDebug("Invoking task-assignment service to retrieve assignments for AssignmentId: {AssignmentId}", id);

        TaskAssignmentResponseDto taskAssignment = await _taskAssignmentService.GetTaskAssignmentByIdAsync(id);

        return ResponseBuilder.CreateResponseSuccess(StatusCodes.Status200OK,taskAssignment);
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult> UpdateTaskAssignmentById(int id, [FromBody] TaskAssignmentUpdateDto updateTaskAssignmentDto)
    {
        if(!ModelState.IsValid || id < 1)
        {
            return ResponseBuilder.CreateResponse(StatusCodes.Status400BadRequest,AppConstants.Errors.ValidationFailed,ModelState);
        }
        _logger.LogDebug("Invoking task-assignment service to modify records for AssignmentId: {AssignmentId}", id);
        
        TaskAssignmentResponseDto updatedTaskAssignment = await _taskAssignmentService.UpdateTaskAssignmentAsync(id, updateTaskAssignmentDto);
        
        return ResponseBuilder.CreateResponseSuccess(StatusCodes.Status200OK,updatedTaskAssignment);
    }
}
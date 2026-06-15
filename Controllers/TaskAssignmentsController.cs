using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.TaskAssignments.DTOs;
using TraineeManagementApi.TaskAssignments.ServiceInterface;

namespace TraineeManagementApi.TaskAssignments.Controller;

[ApiController]
[Route("api/task-assignments")]
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
    public async Task<ActionResult<TaskAssignmentResponseDto>> CreateTaskAssignment([FromBody] TaskAssignmentCreateDto createTaskAssignmentDto)
    {
        _logger.LogDebug("Invoking task-assignment service to add a new task-assignment");
        TaskAssignmentResponseDto createdTaskAssignment = await _taskAssignmentService.CreateTaskAssignmentAsync(createTaskAssignmentDto);
        return CreatedAtAction(nameof(GetTaskAssignmentById), new { id = createdTaskAssignment.Id }, createdTaskAssignment);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskAssignmentResponseDto>>> GetTaskAssignments()
    {
        _logger.LogDebug("Invoking task-assignment service to fetch all task-assignments");
        IEnumerable<TaskAssignmentResponseDto> taskAssignments = await _taskAssignmentService.GetTaskAssignmentsAsync();
        return Ok(taskAssignments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskAssignmentResponseDto>> GetTaskAssignmentById(int id)
    {
        _logger.LogDebug("Invoking task-assignment service to retrieve assignments for AssignmentId: {AssignmentId}", id);
        TaskAssignmentResponseDto taskAssignment = await _taskAssignmentService.GetTaskAssignmentByIdAsync(id);
        return Ok(taskAssignment);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskAssignmentResponseDto>> UpdateTaskAssignmentById(int id, [FromBody] TaskAssignmentUpdateDto updateTaskAssignmentDto)
    {
        _logger.LogDebug("Invoking task-assignment service to modify records for AssignmentId: {AssignmentId}", id);
        TaskAssignmentResponseDto updatedTaskAssignment = await _taskAssignmentService.UpdateTaskAssignmentAsync(id, updateTaskAssignmentDto);
        return Ok(updatedTaskAssignment);
    }
}
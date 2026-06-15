using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.LearningTasks.DTOs;
using TraineeManagementApi.LearningTasks.ServiceInterface;

namespace TraineeManagementApi.LearningTasks.Controller;

[ApiController]
[Route("api/learning-tasks")]
[Authorize]
public class LearningTasksController : ControllerBase
{
    private readonly ILearningTaskService _learningTaskService;
    private readonly ILogger<LearningTasksController> _logger;

    public LearningTasksController(ILearningTaskService learningTaskService, ILogger<LearningTasksController> logger)
    {
        _learningTaskService = learningTaskService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LearningTaskResposeDto>>> GetLearningTasks()
    {
        _logger.LogDebug("Invoking learning-task service to fetch all tasks");
        IEnumerable<LearningTaskResposeDto> learningTasks = await _learningTaskService.GetLearningTaskAsync();
        return Ok(learningTasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LearningTaskResposeDto>> GetLearningTaskById(int id)
    {
        _logger.LogDebug("Invoking learning-task service to retrieve for TaskId: {TaskId}", id);
        LearningTaskResposeDto learningTask = await _learningTaskService.GetLearningTaskByIdAsync(id);
        return Ok(learningTask);
    }

    [HttpPost]
    public async Task<ActionResult<LearningTaskResposeDto>> CreateLearningTask([FromBody] LearningTaskCreateDto createTaskDto)
    {
        _logger.LogDebug("Invoking learning-task service to add a new task");
        LearningTaskResposeDto createdTask = await _learningTaskService.CreateLearningTaskAsync(createTaskDto);
        return Ok(createdTask);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteLearningTask(int id)
    {
        _logger.LogDebug("Invoking learning-task service to delete task for TaskId: {TaskId}", id);
        await _learningTaskService.DeleteLearningTaskByIdAsync(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LearningTaskResposeDto>> UpdateLearningTask(int id, [FromBody] LearningTaskUpdateDto updateTaskDto)
    {
        _logger.LogDebug("Invoking learning-task service to modify task for TaskId: {TaskId}", id);
        LearningTaskResposeDto updatedTask = await _learningTaskService.UpdateLearningTaskByIdAsync(id, updateTaskDto);
        return Ok(updatedTask);
    }
}
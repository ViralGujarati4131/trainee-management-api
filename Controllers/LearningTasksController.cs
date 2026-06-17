using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.LearningTasks.DTOs;
using TraineeManagementApi.LearningTasks.ServiceInterface;
using TraineeManagementApi.ResponsesBuilder;
using TraineeManagementApi.Constants;

namespace TraineeManagementApi.LearningTasks.Controller;

[ApiController]
[Route(AppConstants.Routes.LearningTasks)]
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
        return ResponseBuilder.CreateResponseSuccess(StatusCodes.Status200OK,learningTasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LearningTaskResposeDto>> GetLearningTaskById(int id)
    {
        _logger.LogDebug("Invoking learning-task service to retrieve for TaskId: {TaskId}", id);
        LearningTaskResposeDto learningTask = await _learningTaskService.GetLearningTaskByIdAsync(id);
        return ResponseBuilder.CreateResponseSuccess(StatusCodes.Status200OK,learningTask);
    }

    [HttpPost]
    public async Task<ActionResult<LearningTaskResposeDto>> CreateLearningTask([FromBody] LearningTaskCreateDto createTaskDto)
    {
        if(!ModelState.IsValid)
        {
            return ResponseBuilder.CreateResponse(StatusCodes.Status400BadRequest,AppConstants.Errors.ValidationFailed,ModelState);
        }
        _logger.LogDebug("Invoking learning-task service to add a new task");
        LearningTaskResposeDto createdTask = await _learningTaskService.CreateLearningTaskAsync(createTaskDto);
        return ResponseBuilder.CreateResponseSuccess(StatusCodes.Status200OK,createdTask);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteLearningTask(int id)
    {
        _logger.LogDebug("Invoking learning-task service to delete task for TaskId: {TaskId}", id);
        await _learningTaskService.DeleteLearningTaskByIdAsync(id);
        return ResponseBuilder.CreateResponseSuccess(StatusCodes.Status204NoContent);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LearningTaskResposeDto>> UpdateLearningTask(int id, [FromBody] LearningTaskUpdateDto updateTaskDto)
    {
        if(!ModelState.IsValid)
        {
            return ResponseBuilder.CreateResponse(StatusCodes.Status400BadRequest,AppConstants.Errors.ValidationFailed,ModelState);
        }
        _logger.LogDebug("Invoking learning-task service to modify task for TaskId: {TaskId}", id);
        LearningTaskResposeDto updatedTask = await _learningTaskService.UpdateLearningTaskByIdAsync(id, updateTaskDto);
        return ResponseBuilder.CreateResponseSuccess(StatusCodes.Status200OK,updatedTask);
    }
}
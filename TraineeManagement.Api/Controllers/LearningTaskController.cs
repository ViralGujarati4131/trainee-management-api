using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Data.LearningTaskDTO;
using TraineeManagement.Api.LearningTaskServiceInterface;
using TraineeManagement.Api.ResponsesBuilder;
using TraineeManagement.Api.Data.ConstRoute;
using TraineeManagement.Api.Data.Response;

namespace TraineeManagement.Api.LearningTaskController;

[ApiController]
[Route(CustomConstRoute.LearningTasks)]
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
    public async Task<ActionResult> GetLearningTasks()
    {
        _logger.LogDebug("Invoking learning-task service to fetch all tasks");

        IEnumerable<LearningTaskResposeDto> learningTasks = await _learningTaskService.GetLearningTaskAsync();

        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.Success,
            learningTasks
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetLearningTaskById(int id)
    {
        if (!ModelState.IsValid || id < 1)
        {
            return CustomResponseBuilder.CreateValidationErrorResponse();
        }
        _logger.LogDebug("Invoking learning-task service to retrieve for TaskId: {TaskId}", id);

        LearningTaskResposeDto learningTask = await _learningTaskService.GetLearningTaskByIdAsync(id);

        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.Success,
            learningTask
        );
    }

    [HttpPost]
    public async Task<ActionResult> CreateLearningTask([FromBody] LearningTaskCreateDto createTaskDto)
    {
        if (!ModelState.IsValid)
        {
            return CustomResponseBuilder.CreateValidationErrorResponse();
        }
        _logger.LogDebug("Invoking learning-task service to add a new task");

        LearningTaskResposeDto createdTask = await _learningTaskService.CreateLearningTaskAsync(createTaskDto);

        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.Created, 
            createdTask
        );
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteLearningTask(int id)
    {
        if (!ModelState.IsValid || id < 1)
        {
            return CustomResponseBuilder.CreateValidationErrorResponse();
        }
        _logger.LogDebug("Invoking learning-task service to delete task for TaskId: {TaskId}", id);

        await _learningTaskService.DeleteLearningTaskByIdAsync(id);

        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.NoContent
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateLearningTask(int id, [FromBody] LearningTaskUpdateDto updateTaskDto)
    {
        if (!ModelState.IsValid || id < 1)
        {
            return CustomResponseBuilder.CreateValidationErrorResponse();
        }
        _logger.LogDebug("Invoking learning-task service to modify task for TaskId: {TaskId}", id);

        LearningTaskResposeDto updatedTask = await _learningTaskService.UpdateLearningTaskByIdAsync(id, updateTaskDto);

        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.Updated, 
            updatedTask
        );
    }
}
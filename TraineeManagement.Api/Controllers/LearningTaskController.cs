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

        _logger.LogInformation("State check: Bulk fetch tasks success.");
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataRetrivedSuccess,
            learningTasks
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetLearningTaskById(int id)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        if(id < 1)
        {
            _logger.LogWarning("Request failed validation. Invalid ID range. Id: {TaskId}", id);
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }
        _logger.LogDebug("Invoking learning-task service to retrieve for TaskId: {TaskId}", id);

        LearningTaskResposeDto learningTask = await _learningTaskService.GetLearningTaskByIdAsync(id);

        _logger.LogInformation("State check: Fetch task by ID success. Id: {TaskId}", id);
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataRetrivedSuccess,
            learningTask
        );
    }

    [HttpPost]
    public async Task<ActionResult> CreateLearningTask([FromBody] LearningTaskCreateDto createTaskDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        _logger.LogDebug("Invoking learning-task service to add a new task");

        LearningTaskResposeDto createdTask = await _learningTaskService.CreateLearningTaskAsync(createTaskDto);

        _logger.LogInformation("State check: Task creation success. Id: {TaskId}", createdTask.Id);
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataInsertedSuccess, 
            createdTask
        );
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteLearningTask(int id)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        if(id < 1)
        {
            _logger.LogWarning("Request failed validation. Invalid ID range. Id: {TaskId}", id);
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }
        _logger.LogDebug("Invoking learning-task service to delete task for TaskId: {TaskId}", id);

        await _learningTaskService.DeleteLearningTaskByIdAsync(id);

        _logger.LogInformation("State check: Task deletion success. Id: {TaskId}", id);
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataDeletedNoContent
        );
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateLearningTask(int id, [FromBody] LearningTaskUpdateDto updateTaskDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        if(id < 1)
        {
            _logger.LogWarning("Request failed validation. Invalid ID range. Id: {TaskId}", id);
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }
        _logger.LogDebug("Invoking learning-task service to modify task for TaskId: {TaskId}", id);

        LearningTaskResposeDto updatedTask = await _learningTaskService.UpdateLearningTaskByIdAsync(id, updateTaskDto);

        _logger.LogInformation("State check: Task modification success. Id: {TaskId}", id);
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataUpdatedSuccess, 
            updatedTask
        );
    }
}
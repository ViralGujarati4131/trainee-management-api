using TraineeManagementApi.TaskAssignments.DTOs;

namespace TraineeManagementApi.TaskAssignments.ServiceInterface;

public interface ITaskAssignmentService
{
    Task<TaskAssignmentResponseDto> CreateTaskAssignmentAsync(TaskAssignmentCreateDto createTraineeDto);

    Task<IEnumerable<TaskAssignmentResponseDto>> GetTaskAssignmentsAsync();

    Task<TaskAssignmentResponseDto> GetTaskAssignmentByIdAsync(int id);

    Task<TaskAssignmentResponseDto> UpdateTaskAssignmentAsync(int id, TaskAssignmentUpdateDto updateTaskAssignmentDto);
}
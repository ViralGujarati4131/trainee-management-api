using TraineeManagementApi.LearningTasks.DTOs;

namespace TraineeManagementApi.LearningTasks.ServiceInterface;

public interface ILearningTaskService
{
    public Task<IEnumerable<LearningTaskResposeDto>> GetLearningTaskAsync();

    public Task<LearningTaskResposeDto> GetLearningTaskByIdAsync(int id);

    public Task<LearningTaskResposeDto> CreateLearningTaskAsync(LearningTaskCreateDto createLearningTask);

    public Task DeleteLearningTaskByIdAsync(int id);

    public Task<LearningTaskResposeDto> UpdateLearningTaskByIdAsync(int id,LearningTaskUpdateDto updateLearningTask);
}
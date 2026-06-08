using TraineeManagementApi.DTOs;

namespace TraineeManagementApi.Service.Interface;

public interface ITraineeService
{
    Task<List<TraineeResponseDto>> GetTraineeService();

    Task<TraineeResponseDto?> GetTraineeByIdService(int id);

    Task<TraineeResponseDto> CreateTraineeService(CreateTraineeDto trainee);

    Task<TraineeResponseDto?> UpdateTraineeService(int id, UpdateTraineeDto trainee);

    Task<bool> DeleteTraineeByIdService(int id);

    Task<List<TraineeResponseDto>> SearchTraineesService(string search);
}
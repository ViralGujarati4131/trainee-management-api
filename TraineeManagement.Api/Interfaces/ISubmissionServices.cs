using TraineeManagementApi.Submissions.DTOs;

namespace TraineeManagementApi.Submissions.ServiceInterface;

public interface ISubmissionService
{
    Task<SubmissionResponseDto> CreateSubmissionAsync(SubmissionCreateDto createSubmissionDto);

    Task<IEnumerable<SubmissionResponseDto>> GetSubmissionsAsync();

    Task<SubmissionResponseDto> GetSubmissionByIdAsync(int id);
}
using TraineeManagement.Api.Data.SubmissionDTO;

namespace TraineeManagement.Api.SubmissionServiceInterface;

public interface ISubmissionService
{
    Task<SubmissionResponseDto> CreateSubmissionAsync(SubmissionCreateDto createSubmissionDto);

    Task<IEnumerable<SubmissionResponseDto>> GetSubmissionsAsync();

    Task<SubmissionResponseDto> GetSubmissionByIdAsync(int id);
}
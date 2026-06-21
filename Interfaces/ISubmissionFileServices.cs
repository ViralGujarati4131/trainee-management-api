using TraineeManagementApi.SubmissionFiles.DTOs;

namespace TraineeManagementApi.SubmissionFiles.ServiceInterface;

public interface ISubmissionFileService
{
    Task<SubmissionFileResponseDto> AddSubmissionFileMetaDataAsync(int submissionId,IFormFile file,string storedName);
}
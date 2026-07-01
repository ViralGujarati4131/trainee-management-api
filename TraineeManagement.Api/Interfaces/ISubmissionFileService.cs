using TraineeManagement.Api.Data.SubmissionProcessingContarct;
using TraineeManagement.Api.Data.SubmissionFileDTO;

namespace TraineeManagement.Api.SubmissionFileServiceInterface;

public interface ISubmissionFileService
{
    Task<SubmissionFileResponseDto> AddSubmissionFileMetaDataAsync(int submissionId,IFormFile file,string storedName);
    public Task<SubmissionPublishResult> RequestProcessing(int submissionId, int fileId);
}
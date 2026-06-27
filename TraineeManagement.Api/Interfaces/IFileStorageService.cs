namespace TraineeManagement.Api.FileStorageServiceInterface;
public interface IFileStorageService
{
    Task<string> SaveAsync(int submissionId,IFormFile file);
    Task<(Stream FileStream, string ContentType, string OriginalFileName)> OpenAsync(int id);
    Task<bool> ExistsAsync(int submissionId,IFormFile file);
    Task DeleteAsync(int id);
}
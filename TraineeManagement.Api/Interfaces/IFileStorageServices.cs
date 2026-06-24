namespace TraineeManagementApi.FileStorage.ServiceInterface;
public interface IFileStorageService
{
    Task<string> SaveAsync(int submissionId,IFormFile file);
    Task<(Stream FileStream, string ContentType, string OriginalFileName)> OpenAsync(int id);
    Task<bool> Exists(int submissionId,IFormFile file);
    Task<bool> DeleteAsync(int id);
}
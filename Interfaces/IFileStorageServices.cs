namespace TraineeManagementApi.FileStorage.ServiceInterface;
public interface IFileStorageService
{
    Task<string> SaveAsync(Stream fileStream, string originalFileName);
    Task<Stream> OpenReadAsync(string storageFileName);
    Task<bool> ExistsAsync(string storageFileName);
    Task DeleteAsync(string storageFileName);
}
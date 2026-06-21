using Microsoft.Extensions.Options;
using TraineeManagementApi.FileStorage.Configurations;
using TraineeManagementApi.FileStorage.ServiceInterface;
using TraineeManagementApi.Utils.CustomException;

namespace TraineeManagementApi.FileStorage.Service;

public class FileStorageService : IFileStorageService
{
    private readonly string _rootPath;
    private readonly ILogger<FileStorageService> _logger;
    private readonly FileStorageConfiguration _fileConfiguration;

    public FileStorageService(IWebHostEnvironment env, ILogger<FileStorageService> logger, IOptions<FileStorageConfiguration> fileConfiguration)
    {
        _logger = logger;
        _fileConfiguration = fileConfiguration.Value;

        if (string.IsNullOrWhiteSpace(_fileConfiguration.RootPath))
        {
            throw new FileStorageConfigurationException();
        }
        
        string configuredPath = _fileConfiguration.RootPath; 
        string basePath = env.ContentRootPath;

        _rootPath = Path.Combine(basePath, configuredPath);

        if (!Directory.Exists(_rootPath))
        {
            Directory.CreateDirectory(_rootPath);
        }
    }

    public async Task<string> SaveAsync(Stream fileStream, string originalFileName)
    {
        string extension = Path.GetExtension(originalFileName).ToLowerInvariant();
        string uniqueId = Guid.NewGuid().ToString("N");
        string datePrefix = DateTime.UtcNow.ToString("yyyy/MM/dd");
        
        string storedFileName = $"{datePrefix}/{uniqueId}{extension}"; 

        string diskSafeFileName = storedFileName.Replace("/", "_");
        
        string filePath = Path.Combine(_rootPath, diskSafeFileName);
        
        try
        {
            using FileStream output = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            
            if (fileStream.CanSeek)
            {
                fileStream.Position = 0;
            }
            
            await fileStream.CopyToAsync(output);
            
            return diskSafeFileName; 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Physical disk IO failure saving file {OriginalFileName}", originalFileName);
            throw;
        }
    }

    public Task<Stream> OpenReadAsync(string storageFileName)
    {
        string filePath = Path.Combine(_rootPath, storageFileName);

        if (!File.Exists(filePath))
        {
            throw new ExceptionFileNotFound();
        }

        return Task.FromResult<Stream>(new FileStream(filePath, FileMode.Open, FileAccess.Read));
    }

    public Task<bool> ExistsAsync(string storageFileName)
    {
        string filePath = Path.Combine(_rootPath, storageFileName);
        return Task.FromResult(File.Exists(filePath));
    }

    public Task DeleteAsync(string storageFileName)
    {
        string filePath = Path.Combine(_rootPath, storageFileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            _logger.LogInformation("Physical storage asset successfully deleted: {FileName}", storageFileName);
        }

        return Task.CompletedTask;
    }
}
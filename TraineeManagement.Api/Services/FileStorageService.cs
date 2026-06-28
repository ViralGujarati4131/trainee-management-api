using System.Security.Cryptography;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using TraineeManagement.Api.Data.FileStoreValidation;
    using TraineeManagement.Api.FileStorageServiceInterface;
    using TraineeManagement.Api.Data.SubmissionFileModel;
    using TraineeManagement.Api.Data.SubmissionModel;
    using TraineeManagement.Api.Data.CustomException;
    using TraineeManagement.Api.Data.DatabaseContext;
    using TraineeManagement.Api.Data.Response;
    using TraineeManagement.Api.ResponsesBuilder;
   
    namespace TraineeManagement.Api.FileStorageService;

    public class FileStorageService : IFileStorageService
    {
        private readonly string _rootPath;
        
        private readonly ILogger<FileStorageService> _logger;

        private readonly CustomFileStoreValidation _fileConfiguration;

        private readonly AppDbContext _context;

        public FileStorageService(ILogger<FileStorageService> logger, IOptions<CustomFileStoreValidation> fileConfiguration,AppDbContext context)
        {
            _logger = logger;
            _fileConfiguration = fileConfiguration.Value;
            _context = context;

            if (string.IsNullOrWhiteSpace(_fileConfiguration.RootPath))
            {
                throw new ConfigurationMissingException(CustomResponse.ConfigurationMissingError);
            }
            
            string configuredPath = _fileConfiguration.RootPath; 
            string basePath = _fileConfiguration.BasePath;

            _rootPath = Path.Combine(basePath, configuredPath);

            if (!Directory.Exists(_rootPath))
            {
                Directory.CreateDirectory(_rootPath);
            }
        }

        public async Task<string> SaveAsync(int submissionId,IFormFile file)
        {
            Submission? submission = await _context.Submissions.FindAsync(submissionId);
            if (submission == null)
                throw new BadRequestException(CustomResponse.DataEntryNotFound);

            if (file.Length == 0)
                throw new BadRequestException(CustomResponse.FileEmpty);

            if (file.Length > _fileConfiguration.MaxFileSizeInBytes)
                throw new BadRequestException(CustomResponse.FileSizeExcced);

            string ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!_fileConfiguration.AllowedExtensions.Contains(ext))
            {
                _logger.LogWarning("Dependency failure: Unauthorized extension block. Extension: {Extension}", ext);
                throw new BadRequestException(CustomResponse.FileExtentionNotAllowed);
            }

            if (_fileConfiguration.MagicNumbers.TryGetValue(ext, out string? hexSignature) && !string.IsNullOrWhiteSpace(hexSignature))
            {
                byte[] expectedSignature = Convert.FromHexString(hexSignature);
                byte[] actualHeader = new byte[expectedSignature.Length];

                using (Stream stream = file.OpenReadStream())
                {
                    await stream.ReadExactlyAsync(actualHeader, 0, expectedSignature.Length);
                }

                if (!actualHeader.SequenceEqual(expectedSignature))
                {
                    _logger.LogWarning("Dependency failure: Integrity check mismatched verification payload.");
                    throw new BadRequestException(CustomResponse.FileContentMismatch);
                }
            }

            string uniqueId = Guid.NewGuid().ToString("N");
            string datePrefix = DateTime.UtcNow.ToString("yyyy/MM/dd");
            string storedFileName = $"{datePrefix}/{uniqueId}{ext}"; 
            string diskSafeFileName = storedFileName.Replace("/", "_");
            string filePath = Path.Combine(_rootPath, diskSafeFileName);
            try
            {
                using FileStream output = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                using Stream fileStream = file.OpenReadStream();
                await fileStream.CopyToAsync(output);
                
                _logger.LogInformation("File write complete. SubmissionId: {SubmissionId}", submissionId);
                return diskSafeFileName;  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Dependency failure: Physical disk IO failure writing file stream. SubmissionId: {SubmissionId}", submissionId);
                throw new IOError(CustomResponse.IOFail);
            }
        }

        public async Task<(Stream FileStream, string ContentType, string OriginalFileName)> OpenAsync(int id)
        {
            SubmissionFile? metadata = await _context.SubmissionFiles.FindAsync(id);
            if (metadata == null)
            {
                throw new BadRequestException(CustomResponse.DataEntryNotFound);
            }

            string filePath = Path.Combine(_rootPath, metadata.StorageFileName);
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("Dependency failure: Physical file missing from disk store. FileId: {FileId}", id);
                throw new FileNotFoundError(CustomResponse.FileNotFound);
            }

            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous);
        
            _logger.LogInformation("File read access initialization complete. FileId: {FileId}", id);
            return (fileStream, metadata.ContentType, metadata.OriginalFileName);
        }

        public async Task<bool> ExistsAsync(int submissionId, IFormFile file)
        {
            using SHA256 sha256 = SHA256.Create();
            using Stream read = file.OpenReadStream();
            byte[] hashBytes = await sha256.ComputeHashAsync(read);
            string checksum = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();  
            bool isDuplicate = await _context.SubmissionFiles
                .AsNoTracking()
                .AnyAsync(s => 
                    s.SubmissionId == submissionId &&
                    s.Checksum == checksum);               
            return isDuplicate;
        }

        public async Task DeleteAsync(int id)
        {
            SubmissionFile? metadata = await _context.SubmissionFiles.FindAsync(id);
            if (metadata == null)
            {
                throw new BadRequestException(CustomResponse.DataEntryNotFound);
            }
            string filePath = Path.Combine(_rootPath, metadata.StorageFileName);

            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                    _logger.LogInformation("Physical storage file successfully deleted. FileId: {FileId}", id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Dependency failure: Erase operation failed on physical disk. FileId: {FileId}", id);
                    throw new IOError(CustomResponse.IOFail);
                }
            }
             _context.SubmissionFiles.Remove(metadata);
            await _context.SaveChangesAsync();
        }
    }
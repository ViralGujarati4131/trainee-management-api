using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.FileStorage.ServiceInterface;
using TraineeManagementApi.SubmissionFiles.Models;
using TraineeManagementApi.Submissions.Models;
using TraineeManagementApi.Utils.ResponsesBuilder;
using TraineeManagementApi.Utils.CustomException;
using TraineeManagementApi.Constants;
using TraineeManagementApi.SubmissionFiles.DTOs;
using TraineeManagementApi.SubmissionFiles.ServiceInterface;
using TraineeManagementApi.FileStorage.Configurations;
using Microsoft.Extensions.Options;

namespace TraineeManagementApi.SubmissionFiles.Controller;

[ApiController]
[Route(AppConstants.Routes.SubmissionFiles)]
[Authorize]
public class SubmissionFilesController : ControllerBase
{
    private readonly ILogger<SubmissionFilesController> _logger;

    private readonly ISubmissionFileService _submissionFileService;

    private readonly IFileStorageService _fileStorageService;

    private readonly FileStorageConfiguration _fileConfiguration;

    private readonly AppDbContext _context;

    public SubmissionFilesController(ILogger<SubmissionFilesController> logger, IFileStorageService fileStorageService, AppDbContext context, ISubmissionFileService submissionFileService, IOptions<FileStorageConfiguration> fileConfiguration)
    {
        _logger = logger;
        _fileStorageService = fileStorageService;
        _context = context;
        _submissionFileService = submissionFileService;
        _fileConfiguration = fileConfiguration.Value;
    }

    [HttpPost("{submissionId}/files")]
    public async Task<IActionResult> UploadFile(int submissionId, IEnumerable<IFormFile> files)
    {
        if (!ModelState.IsValid || submissionId < 1)
        {
            return ResponseBuilder.CreateValidationErrorResponse();
        }
        ICollection<SubmissionFileResponseDto> savedMetadataRecords = new List<SubmissionFileResponseDto>(); 
        
        if (files == null || !files.Any())
        {
            _logger.LogWarning("Upload attempt blocked: No files found in the multipart form-data payload request.");
            throw new BadRequestException("No files were attached to the upload request.");
        }

        Submission? submission = await _context.Submissions.FindAsync(submissionId);
        if (submission == null)
        {
            throw new BadRequestException("The referenced submission profile does not exist.");
        }

        foreach (IFormFile file in files)
        {   
            if (file == null || file.Length == 0)
                throw new BadRequestException("Empty file uploads are not allowed.");

            if (file.Length > _fileConfiguration.MaxFileSizeInBytes)
                throw new BadRequestException($"File size exceeds the allowed {_fileConfiguration.MaxFileSizeInBytes / (1024 * 1024)} MB limit.");

            string ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!_fileConfiguration.AllowedExtensions.Contains(ext))
            {
                _logger.LogWarning("Unauthorized file extension block triggered for: {Extension}", ext);
                throw new BadRequestException($"File type extension '{ext}' is not authorized.");
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
                    _logger.LogWarning("Security: File contents for {FileName} do not match hex signature {Hex}!", file.FileName, hexSignature);
                    throw new BadRequestException("The file contents do not match its true file type extension signature safely.");
                }
            }
            
            string storedName = string.Empty;
            try
            {
                using Stream readStream = file.OpenReadStream();
                storedName = await _fileStorageService.SaveAsync(readStream, file.FileName);;
                SubmissionFileResponseDto submissionFile = await _submissionFileService.AddSubmissionFileMetaDataAsync(submissionId,file,storedName);
                savedMetadataRecords.Add(submissionFile);
            }
            catch (Exception)
            {
               if (!string.IsNullOrWhiteSpace(storedName))
                {
                    _logger.LogError("Database metadata persistence failed for file: {StoredName}.", storedName);
                    await _fileStorageService.DeleteAsync(storedName);
                }
                throw new BadRequestException($"File processing operation failed completely");
            }
        }
        return ResponseBuilder.CreateSuccessResponse(
            AppConstants.ApiResponse.Created,
            savedMetadataRecords
        );        
    }

    [HttpGet("{id}/download")] 
    public async Task<IActionResult> DownloadFile(int id)
    {
        if (!ModelState.IsValid || id < 1)
        {
            return ResponseBuilder.CreateValidationErrorResponse();
        }

        SubmissionFile? metadata = await _context.SubmissionFiles.FindAsync(id);
        if (metadata == null)
        {
            throw new ExceptionFileNotFound();
        }

        Stream stream = await _fileStorageService.OpenReadAsync(metadata.StorageFileName);
        return File(stream, metadata.ContentType, metadata.OriginalFileName);
    }

    [HttpDelete("{id}")] 
    public async Task<IActionResult> DeleteFile(int id)
    {
        if (!ModelState.IsValid || id < 1)
        {
            return ResponseBuilder.CreateValidationErrorResponse();
        }

        SubmissionFile? metadata = await _context.SubmissionFiles.FindAsync(id);
        if (metadata == null)
        {
            throw new ExceptionFileNotFound();
        }

        _context.SubmissionFiles.Remove(metadata);
        await _context.SaveChangesAsync();

        await _fileStorageService.DeleteAsync(metadata.StorageFileName);
        
        return ResponseBuilder.CreateSuccessResponse(
            AppConstants.ApiResponse.NoContent
        );
    }
}
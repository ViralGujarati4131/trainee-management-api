using TraineeManagementApi.SubmissionFiles.DTOs;
using TraineeManagementApi.SubmissionFiles.ServiceInterface;
using System.Security.Cryptography;
using TraineeManagementApi.SubmissionFiles.Models;
using TraineeManagementApi.Constants;

namespace TraineeManagementApi.SubmissionFiles.Service;

public class SubmissionFileService : ISubmissionFileService
{
    private readonly AppDbContext _context;
    
    private readonly ILogger<SubmissionFileService> _logger;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public SubmissionFileService(AppDbContext context, ILogger<SubmissionFileService> logger,IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<SubmissionFileResponseDto> AddSubmissionFileMetaDataAsync(int submissionId,IFormFile file,string storedName)
    {
        string? userId = _httpContextAccessor.HttpContext?.User?.FindFirst(AppConstants.Security.ClaimId)?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogError("Access Denied: Attempted to upload file metadata without a valid User ID claim in the bearer token.");
        
            throw new UnauthorizedAccessException("User identification claim is missing or invalid.");
        }
        
        string checksum;
        using (SHA256 sha256 = SHA256.Create())
        {
            using Stream readStream = file.OpenReadStream();
            byte[] hashBytes = await sha256.ComputeHashAsync(readStream);
            checksum = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();    
        } 

        SubmissionFile submissionFile = new SubmissionFile
        {
            SubmissionId = submissionId,
            OriginalFileName = file.FileName,
            StorageFileName = storedName,
            ContentType = file.ContentType,
            Size = file.Length,
            Checksum = checksum,
            UploadedByUserId = userId
        };
         _context.SubmissionFiles.Add(submissionFile);
        await _context.SaveChangesAsync();

        return new SubmissionFileResponseDto(
                submissionFile.SubmissionId,
                submissionFile.OriginalFileName,
                submissionFile.ContentType,
                submissionFile.Size,
                submissionFile.UploadedByUserId
        );
    }
}
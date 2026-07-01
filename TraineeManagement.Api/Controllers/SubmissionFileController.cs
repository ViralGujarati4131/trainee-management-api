using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.FileStorageServiceInterface;
using TraineeManagement.Api.ResponsesBuilder;
using TraineeManagement.Api.Data.ConstRoute;
using TraineeManagement.Api.Data.Response;
using TraineeManagement.Api.Data.SubmissionFileDTO;
using TraineeManagement.Api.SubmissionFileServiceInterface;
using TraineeManagement.Api.Data.SubmissionProcessingContarct;
using TraineeManagement.Api.Data.DatabaseContext;

namespace TraineeManagement.Api.SubmissionFileController;

[ApiController]
[Route(CustomConstRoute.SubmissionFiles)]
[Authorize]
public class SubmissionFilesController : ControllerBase
{
    private readonly ILogger<SubmissionFilesController> _logger;

    private readonly ISubmissionFileService _submissionFileService;

    private readonly IFileStorageService _fileStorageService;

    private readonly AppDbContext _context;

    public SubmissionFilesController(ILogger<SubmissionFilesController> logger, IFileStorageService fileStorageService, AppDbContext context, ISubmissionFileService submissionFileService)
    {
        _logger = logger;
        _fileStorageService = fileStorageService;
        _context = context;
        _submissionFileService = submissionFileService;
    }

    [HttpPost("/api/submission/{submissionId}/files")]
    public async Task<IActionResult> UploadFile(int submissionId, IFormFile file)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        if(submissionId < 1)
        {
            _logger.LogWarning("Request failed validation. Invalid ID range. SubmissionId: {SubmissionId}", submissionId);
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }
        
        if (file == null)
        {
            _logger.LogWarning("Upload attempt blocked: No files found in the multipart form-data payload request.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.FileNotAttached);
        }

        bool isDuplicates = await _fileStorageService.ExistsAsync(submissionId,file);
        if(isDuplicates)
        {
            _logger.LogWarning("Upload duplicate rejected. SubmissionId: {SubmissionId}", submissionId);
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.FileAlreadyUploaded);
        }
        
        string storedName = await _fileStorageService.SaveAsync(submissionId,file);
        
        SubmissionFileResponseDto submissionFile = await _submissionFileService.AddSubmissionFileMetaDataAsync(submissionId,file,storedName);

        try
        {
            SubmissionPublishResult publishResult = await _submissionFileService.RequestProcessing(submissionId,submissionFile.Id);

            _logger.LogInformation(
                "Published SubmissionProcessingRequested: MessageId={MessageId}, CorrelationId={CorrelationId}, SubmissionId={SubmissionId}, Result={Result}",
                publishResult.MessageId,
                publishResult.CorrelationId,
                submissionId,
                publishResult.Success ? "Success" : "Failed"
            );

            if (!publishResult.Success)
            {
                _logger.LogWarning("Publish failed. MessageId: {MessageId}, SubmissionId: {SubmissionId}", publishResult.MessageId, submissionId);
                return CustomResponseBuilder.CreateSuccessResponse(CustomResponse.UnavailableRabbitMQService);
            }

            _logger.LogInformation("Publish success. MessageId: {MessageId}, JobId: {JobId}", publishResult.MessageId, publishResult.ProcessingJobId);
            return CustomResponseBuilder.CreateSuccessResponse(
                CustomResponse.FileUploadAccepted,
                publishResult.ProcessingJobId
            ); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dependency failure publishing task event. SubmissionId: {SubmissionId}", submissionId);            
            return CustomResponseBuilder.CreateSuccessResponse(CustomResponse.UnavailableRabbitMQService);
        }       
    }

    [HttpGet("{id}/download")] 
    public async Task<ActionResult> DownloadFile(int id)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        if(id < 1)
        {
            _logger.LogWarning("Request failed validation. Invalid ID range. FileId: {FileId}", id);
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }
        (Stream fileStream, string contentType, string originalFileName) = await _fileStorageService.OpenAsync(id);
        
        _logger.LogInformation("State check: File download stream accessed. FileId: {FileId}", id);
        return File(fileStream, contentType, originalFileName);
    }

    [HttpDelete("{id}")] 
    public async Task<IActionResult> DeleteFile(int id)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        if(id < 1)
        {
            _logger.LogWarning("Request failed validation. Invalid ID range. FileId: {FileId}", id);
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }

        await _fileStorageService.DeleteAsync(id);
   
        _logger.LogInformation("State check: File erased from physical store tracking. FileId: {FileId}", id);
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataDeletedNoContent
        );
    }
}
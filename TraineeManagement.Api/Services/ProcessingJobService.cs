using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data.CustomException;
using TraineeManagement.Api.Data.DatabaseContext;
using TraineeManagement.Api.Data.ProcessingJobDto;
using TraineeManagement.Api.ProcessingJobServiceInterface;
using TraineeManagement.Api.Data.Response;
using TraineeManagement.Api.ResponsesBuilder;
using Microsoft.Extensions.Logging;

namespace TraineeManagement.Api.ProcessingJobService;

public class ProcessingJobService : IProcessingJobService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProcessingJobService> _logger;

    public ProcessingJobService(AppDbContext context, ILogger<ProcessingJobService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ProcessingJobResponseDto> GetProcessingJobByIdAsync(int id)
    {
        _logger.LogInformation("Fetching job status. JobId: {JobId}", id);

        ProcessingJobResponseDto? jobTrack = await _context.ProcessingJobs
            .Where(pj => pj.Id == id)
            .Select(j => new ProcessingJobResponseDto(
                j.Id,
                j.MessageId,
                j.CorrelationId,
                j.SubmissionId,
                j.Status,
                j.Attempts,
                j.ErrorSummary,
                j.RequestedAt,
                j.StartedAt,
                j.CompletedAt
            ))
            .FirstOrDefaultAsync();

        if (jobTrack == null)
        {
            _logger.LogWarning("Dependency failure: Job record missing. JobId: {JobId}", id);
            throw new NotFoundException(CustomResponse.NotFound,"Processing Job");
        }

        _logger.LogInformation("Job record found. JobId: {JobId}, Status: {Status}", id, jobTrack.Status);
        return jobTrack;
    }
}
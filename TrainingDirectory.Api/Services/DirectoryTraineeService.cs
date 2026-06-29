using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data.CustomException;
using TraineeManagement.Api.Data.DatabaseContext;
using TraineeManagement.Api.Data.TraineeDTO;
using TrainingDirectory.Api.DirectoryTraineeServiceInterface;
using TraineeManagement.Api.Data.Response;
using TraineeManagement.Api.CorrelationId;
using Microsoft.Extensions.Logging;

namespace TrainingDirectory.Api.DirectoryTraineeService;

public class DirectoryTraineeService : IDirectoryTraineeService
{
    private readonly AppDbContext _context;
    private readonly ILogger<DirectoryTraineeService> _logger;
    private readonly ICorrelationIdAccessor _correlationIdAccessor;

    public DirectoryTraineeService(
        AppDbContext context, 
        ILogger<DirectoryTraineeService> logger,
        ICorrelationIdAccessor correlationIdAccessor)
    {
        _context = context;
        _logger = logger;
        _correlationIdAccessor = correlationIdAccessor;
    }

    public async Task<TraineeResponseDto> GetTraineeByIdAsync(int id, CancellationToken cancellationToken)
    {
        string correlationId = _correlationIdAccessor.Get();

        _logger.LogInformation(
            "Fetching trainee record. TraineeId: {TraineeId}, CorrelationId: {CorrelationId}", 
            id, correlationId);

        TraineeResponseDto? trainee = await _context.Trainees
            .AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => new TraineeResponseDto(t.Id, t.FirstName, t.LastName))
            .FirstOrDefaultAsync(cancellationToken);

        if (trainee == null)
        {
            _logger.LogWarning(
                "Trainee record missing. TraineeId: {TraineeId}, CorrelationId: {CorrelationId}", 
                id, correlationId);
            throw new NotFoundException(CustomResponse.NotFound, "Trainee");
        }

        _logger.LogInformation(
            "Trainee record found. TraineeId: {TraineeId}, CorrelationId: {CorrelationId}", 
            id, correlationId);

        return trainee;
    }
}
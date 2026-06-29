using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data.CustomException;
using TraineeManagement.Api.Data.DatabaseContext;
using TraineeManagement.Api.Data.TraineeDTO;
using TrainingDirectory.Api.DirectoryTraineeServiceInterface;
using TraineeManagement.Api.Data.Response;

namespace TrainingDirectory.Api.DirectoryTraineeService;

public class DirectoryTraineeService : IDirectoryTraineeService
{
    private readonly AppDbContext _context;
    private readonly ILogger<DirectoryTraineeService> _logger;

    public DirectoryTraineeService(
        AppDbContext context, 
        ILogger<DirectoryTraineeService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TraineeResponseDto> GetTraineeByIdAsync(int id, CancellationToken cancellationToken)
    {

        _logger.LogInformation("Fetching trainee record. TraineeId: {TraineeId}", id);

        TraineeResponseDto? trainee = await _context.Trainees
            .AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => new TraineeResponseDto(t.Id, t.FirstName, t.LastName))
            .FirstOrDefaultAsync(cancellationToken);

        if (trainee == null)
        {
            _logger.LogWarning("Trainee record missing. TraineeId: {TraineeId}",id);
            throw new NotFoundException(CustomResponse.NotFound, "Trainee");
        }

        _logger.LogInformation("Trainee record found. TraineeId: {TraineeId}", id);

        return trainee;
    }
}
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

    public DirectoryTraineeService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<TraineeResponseDto> GetTraineeByIdAsync(int id, CancellationToken cancellationToken)
    {
        TraineeResponseDto? trainee = await _context.Trainees
            .AsNoTracking()
            .Where(t => t.Id == id)
            .Select(t => new TraineeResponseDto(t.Id, t.FirstName, t.LastName))
            .FirstOrDefaultAsync(cancellationToken);

        if (trainee == null)
        {
            throw new NotFoundException(CustomResponse.NotFound,"Trainee");
        }
        return trainee;
    }
}
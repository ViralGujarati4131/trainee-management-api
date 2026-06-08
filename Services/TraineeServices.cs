using Microsoft.EntityFrameworkCore;
using TraineeManagementApi.DTOs;
using TraineeManagementApi.Models;
using TraineeManagementApi.Service.Interface;

namespace TraineeManagementApi.Service;

public class TraineeService : ITraineeService
{

    private readonly AppDbContext context;
    public TraineeService(AppDbContext c){
        context = c;
    }

    private TraineeResponseDto MakeResponse(Trainee t){
        return new TraineeResponseDto
        {
            Id = t.Id,
            FirstName = t.FirstName,
            LastName = t.LastName,
        };
    }

    public async Task<List<TraineeResponseDto>> GetTraineeService()
    {
        List<Trainee> t = await context.Trainees.ToListAsync();
        List<TraineeResponseDto> res = t.Select(u => MakeResponse(u)).ToList();
        return res;
    }
    public async Task<TraineeResponseDto?> GetTraineeByIdService(int id)
    {
        var traineeGet = await context.Trainees.FindAsync(id);
        if(traineeGet == null) return null;
        var res = MakeResponse(traineeGet);
        return res;
    }
     
     public async Task<TraineeResponseDto> CreateTraineeService(CreateTraineeDto newTrainee)
    {
        var createTrainee = new Trainee
        {
            FirstName = newTrainee.FirstName,
            LastName = newTrainee.LastName,
            Email = newTrainee.Email,
            TechStack = newTrainee.TechStack,
            Status = newTrainee.Status,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };
        await context.Trainees.AddAsync(createTrainee);
        await context.SaveChangesAsync();
        var res = MakeResponse(createTrainee);
        return res;
    } 
    public async Task<TraineeResponseDto?> UpdateTraineeService(int id,UpdateTraineeDto updateTrainee)
    {
        var trainee = context.Trainees.Find(id);
        if(trainee == null) return null;
        trainee.FirstName = updateTrainee.FirstName;
        trainee.LastName = updateTrainee.LastName;
        trainee.Email = updateTrainee.Email;
        trainee.TechStack = updateTrainee.TechStack;
        trainee.Status = updateTrainee.Status;
        trainee.UpdatedDate = DateTime.Now;
        await context.SaveChangesAsync();
        var res = MakeResponse(trainee);
        return res;
    }
    public async Task<bool> DeleteTraineeByIdService(int id)
    {
        var traineeDelete = await context.Trainees.FindAsync(id);
        if(traineeDelete == null)
        {
            return false;
        }
        context.Trainees.Remove(traineeDelete);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<List<TraineeResponseDto>> SearchTraineesService(string search)
    {
        var query = search.ToLower();
        var res = await context.Trainees.Where
                    (t => (t.FirstName ?? "").ToLower().Contains(query) ||
                    (t.LastName  ?? "").ToLower().Contains(query) ||
                    (t.Email     ?? "").ToLower().Contains(query) ||
                    (t.TechStack ?? "").ToLower().Contains(query))
        .ToListAsync();
        List<TraineeResponseDto> t = res.Select(u => MakeResponse(u)).ToList();
        return t;
    }
}


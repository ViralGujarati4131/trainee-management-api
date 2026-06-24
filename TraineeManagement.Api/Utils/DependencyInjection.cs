using Microsoft.Extensions.DependencyInjection;
using TraineeManagementApi.LearningTasks.Service;
using TraineeManagementApi.LearningTasks.ServiceInterface;
using TraineeManagementApi.Mentors.Service;
using TraineeManagementApi.Mentors.ServiceInterface;
using TraineeManagementApi.Trainees.Service;
using TraineeManagementApi.Trainees.ServiceInterface;
using TraineeManagementApi.Users.Service;
using TraineeManagementApi.Users.ServiceInterface;
using TraineeManagementApi.TaskAssignments.ServiceInterface;
using TraineeManagementApi.TaskAssignments.Service;
using TraineeManagementApi.Submissions.ServiceInterface;
using TraineeManagementApi.Submissions.Service;
using TraineeManagementApi.Reviews.ServiceInterface;
using TraineeManagementApi.Reviews.Service;
using TraineeManagementApi.FileStorage.ServiceInterface;
using TraineeManagementApi.FileStorage.Service;
using TraineeManagementApi.SubmissionFiles.ServiceInterface;
using TraineeManagementApi.SubmissionFiles.Service;
using TraineeManagementApi.RedisCaching.ServiceInterface;
using TraineeManagementApi.RedisCaching.Service;
using TraineeManagementApi.Utils.JwtService;

namespace Microsoft.Extensions.DependencyInjection; 


public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITraineeService, TraineeService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IMentorServices, MentorService>();
        services.AddScoped<ILearningTaskService, LearningTaskService>();
        services.AddScoped<ITaskAssignmentService, TaskAssignmentService>();
        services.AddScoped<ISubmissionService, SubmissionService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<ISubmissionFileService, SubmissionFileService>();
        services.AddScoped<ICacheService, CacheService>();

        return services;
    }
}
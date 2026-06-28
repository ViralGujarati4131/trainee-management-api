using TraineeManagement.Api.Data.Constants;
using TraineeManagement.Api.Data.FileStoreValidation;

namespace TraineeManagement.Api.Extensions;

public static class FileStorageExtensions
{
    public static IServiceCollection AddAppFileStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CustomFileStoreValidation>(
            configuration.GetSection(AppConstants.ConfigSections.FileStorage));

        return services;
    }
}
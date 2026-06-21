using TraineeManagementApi.Constants;

namespace TraineeManagementApi.Utils.CustomException;

public class BaseApplicationException : Exception
{
    public ApiResponseDescriptor Descriptor { get; }

    public BaseApplicationException(ApiResponseDescriptor descriptor, string? customMessage = null) 
        : base(customMessage ?? descriptor.Message)
    {
        Descriptor = descriptor with { Message = customMessage ?? descriptor.Message };
    }
}

public class NotFoundException : BaseApplicationException
{
    public NotFoundException(string? entityName = null) 
        : base(AppConstants.ApiResponse.NotFound, entityName != null ? $"{entityName} record was not found." : null) {}
}

public class UnauthorizedException : BaseApplicationException
{
    public UnauthorizedException() 
        : base(AppConstants.ApiResponse.Unauthorized) {}
}

public class BadRequestException : BaseApplicationException
{
    public BadRequestException(string customMessage) 
        : base(AppConstants.ApiResponse.BadRequest, customMessage) {}
}

public class ExceptionFileNotFound : BaseApplicationException
{
    public ExceptionFileNotFound() 
        : base(AppConstants.ApiResponse.FileNotFound) {}
}

public class JwtOperationException : BaseApplicationException
{
    public JwtOperationException() 
        : base(AppConstants.ApiResponse.JwtAuthError) {}
}

public class JwtSecretMissingException : BaseApplicationException
{
    public JwtSecretMissingException() 
        : base(AppConstants.ApiResponse.JwtSecretMissing) {}
}

public class FileStorageConfigurationException : BaseApplicationException
{
    public FileStorageConfigurationException() 
        : base(AppConstants.ApiResponse.FileStorageConfigError) {}
}

public class DataSeedingException : BaseApplicationException
{
    public DataSeedingException() 
        : base(AppConstants.ApiResponse.DataSeedingError) {}
}
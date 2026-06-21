namespace TraineeManagementApi.Constants;

public static class AppConstants
{
    public static class ApiResponse
    {
        //  SUCCESS ANSWERS 
        public static readonly ApiResponseDescriptor LoginSuccess = 
            new(200, "2000", "User authenticated successfully. Security token issued.");

        public static readonly ApiResponseDescriptor Success = 
            new(200, "2000", "Data retrieved successfully");
            
        public static readonly ApiResponseDescriptor Created = 
            new(201, "2010", "Data created successfully");
            
        public static readonly ApiResponseDescriptor Updated = 
            new(200, "2020", "Data updated successfully");

        public static readonly ApiResponseDescriptor NoContent = 
            new(204, "2040", "No content found for this request");

        //  CLIENT SIDE ERRORS  
        public static readonly ApiResponseDescriptor BadRequest = 
            new(400, "4000", "Invalid request arguments or operation rules");
            
        public static readonly ApiResponseDescriptor ValidationError = 
            new(400, "4010", "Validation failed for the request data");
            
        public static readonly ApiResponseDescriptor Unauthorized = 
            new(401, "4030", "Invalid credential or authorization missing");
            
        public static readonly ApiResponseDescriptor NotFound = 
            new(404, "4040", "Requested data was not found");
            
        public static readonly ApiResponseDescriptor FileNotFound = 
            new(404, "4045", "The requested physical file could not be located");

        //  DATABASE SPECIFIC CLIENT ERRORS 
        public static readonly ApiResponseDescriptor SqlReferenceConflict = 
            new(400, "4610", "Some of the provided reference identifiers conflict with data rules");
            
        public static readonly ApiResponseDescriptor SqlDeleteReferenceError = 
            new(400, "4620", "Delete operation could not be completed because of existing data references");
            
        public static readonly ApiResponseDescriptor UsernameExists = 
            new(400, "4630", "Username already exists within the trainee network");

        // SERVER ERRORS 
        public static readonly ApiResponseDescriptor InternalServerError = 
            new(500, "5000", "Something went wrong internally, please try again");
            
        public static readonly ApiResponseDescriptor JwtAuthError = 
            new(500, "5010", "An unexpected error occurred while processing encryption signatures");
            
        public static readonly ApiResponseDescriptor JwtSecretMissing = 
            new(500, "5015", "Critical configuration mismatch: JWT Secret Key not configured");

        public static readonly ApiResponseDescriptor FileStorageConfigError = 
            new(500, "5001", "File storage subsystem is misconfigured.");
            
        public static readonly ApiResponseDescriptor DataSeedingError = 
            new(500, "5002", "System data seeding failed.");
    }

    public static class Routes
    {
        public const string Health = "api/health";
        public const string LearningTasks = "api/learning-tasks";
        public const string Mentors = "api/mentors";
        public const string Reviews = "api/reviews";
        public const string Submissions = "api/submissions";
        public const string Trainees = "api/trainee";
        public const string TaskAssignments = "api/task-assignments";
        public const string SubmissionFiles = "api/submission-files";
        public const string Auth = "api/auth";
        public const string PaginationSearch = "paginationSearch";
        public const string Login = "login";
    }

    public static class Database
    {
        public static class MySqlErrorCodes
        {
            public const int NotFoundReference = 1452;
            public const int DeleteReference = 1451;
            public const int UsernameExists = 1062;
        }
    }

    public static class Security
    {
        public const string ClaimId = "Id"; 
        public const string ClaimUsername = "Username";
        public const string ClaimRole = "Role";
        public const string DefaultRole = "Admin";
        public const int DefaultExpiryMinutes = 60;

        public static class Seeding
        {
            public const string DefaultAdminUsername = "admin";
            public const string DefaultAdminPassword = "Admin@123";
        }
    }
    public static class ConfigSections
    {
        public const string FileStorage = "FileStorage";
    }
}
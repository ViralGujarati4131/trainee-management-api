namespace TraineeManagementApi.Constants
{
    public static class AppConstants
    {
        public static class Errors
        {
            public const string ValidationFailed = "Validation failed";
            public const string AllFieldsRequired = "All fields are require";
            
            // Middleware Errors
            public const string JwtAuthError = "An unexpected error occurred while processing authentication please retry";
            public const string SqlReferenceConflict = "Some of the provided References does conflits ";
            public const string SqlDeleteReferenceError = "Delete Operation Could not be completed because of existing reference";
            public const string UsernameExists = "Username is already exists";
            public const string GeneralInternalServerError = "Something Went Wrong, Please Try Again";

            // Jwtservice 
            public const string JwtSecretMissing = "JWT Secret Key not configured.";

            public static class LearningTasks
            {
                public const string NotFound = "LearningTask was not found";
            }

            public static class Mentors
            {
                public const string NotFound = "Mentor was not found";
            }

            public static class Reviews
            {
                public const string NotFound = "Review was not found";
            }

            public static class Submissions
            {
                public const string NotFound = "Submission was not found";
            }

            public static class TaskAssignments
            {
                public const string NotFound = "TaskAssignment was not found";
                public const string ModelInvalidDueDate = "DueDate cannot be earlier than AssignedDate.";
            }

            public static class Users
            {
                public const string NotFound = "User was not found";
                public const string InvalidCredentials = "Invalid credential provided";
            }

            public static class Trainees
            {
                public const string NotFound = "Trainee was not found";
            }
        }

        public static class Routes
        {
            public const string LearningTasks = "api/learning-tasks";
            public const string Mentors = "api/mentors";
            public const string Reviews = "api/reviews";
            public const string Submissions = "api/submissions";
            public const string Trainees = "api/trainee";
            public const string TaskAssignments = "api/task-assignments";
            public const string Auth = "api/auth";
            
            // Sub-routes
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
            // Claims
            public const string ClaimId = "Id";
            public const string ClaimUsername = "Username";
            public const string ClaimRole = "Role";

            // Seed user
            public const string DefaultRole = "Admin";
            public const int DefaultExpiryMinutes = 60;

            // seed admin
            public static class Seeding
            {
                public const string DefaultAdminUsername = "admin";
                public const string DefaultAdminPassword = "Admin@123";
            }
        }
    }
}
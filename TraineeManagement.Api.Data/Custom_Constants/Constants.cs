namespace TraineeManagement.Api.Data.Constants;

public static class AppConstants
{
    
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

        public const string GetRootPath = "RootPath:Path";

    }

    public static class RabbitMQ
    {
        public static string SubmissionProcessing = "submission-processing";

        public static string GetExchange(string queueName) => $"{queueName}.exchange";
        public static string GetQueue(string queueName) => $"{queueName}.queue";
        public static string GetRoutingKey(string queueName) => $"{queueName}.routing-key";
        
        public static string GetDlxExchange(string queueName) => $"{queueName}.dlx";
        public static string GetDlxQueue(string queueName) => $"{queueName}.failed";
        public static string GetDlxRoutingKey(string queueName) => $"{queueName}.failed.routing-key";
    }

}
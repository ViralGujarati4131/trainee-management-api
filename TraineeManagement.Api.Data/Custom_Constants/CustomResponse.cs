using TraineeManagement.Api.Data.ResponseDescriptor;

namespace TraineeManagement.Api.Data.Response;

 public static class CustomResponse
 {
    public static readonly CustomResponseDescriptor LoginSuccess = 
        new(200, "2000", "User authenticated successfully. Security token issued.");

    public static readonly CustomResponseDescriptor Success = 
        new(200, "2000", "Data retrieved successfully");
        
    public static readonly CustomResponseDescriptor Created = 
        new(201, "2010", "Data created successfully");
            
    public static readonly CustomResponseDescriptor Updated = 
        new(200, "2020", "Data updated successfully");

    public static readonly CustomResponseDescriptor NoContent = 
        new(204, "2040", "No content found for this request");


    //  CLIENT SIDE ERRORS  

    public static readonly CustomResponseDescriptor BadRequest = 
        new(400, "4000", "Invalid request arguments or operation rules");
            
    public static readonly CustomResponseDescriptor ValidationError = 
        new(400, "4010", "Validation failed for the request data");
            
    public static readonly CustomResponseDescriptor Unauthorized = 
        new(401, "4030", "Invalid credential or authorization missing");
            
    public static readonly CustomResponseDescriptor NotFound = 
        new(404, "4040", "Requested data was not found");
            
    public static readonly CustomResponseDescriptor FileNotFound = 
        new(404, "4045", "The requested physical file could not be located");


    //  DATABASE SPECIFIC CLIENT ERRORS 

    public static readonly CustomResponseDescriptor SqlReferenceConflict = 
        new(400, "4610", "Some of the provided reference identifiers conflict with data rules");
        
    public static readonly CustomResponseDescriptor SqlDeleteReferenceError = 
        new(400, "4620", "Delete operation could not be completed because of existing data references");
        
    public static readonly CustomResponseDescriptor UsernameExists = 
        new(400, "4630", "Username already exists within the trainee network");

    
    // SERVER ERRORS 
    
    public static readonly CustomResponseDescriptor InternalServerError = 
            new(500, "5000", "Something went wrong internally, please try again");
            
    public static readonly CustomResponseDescriptor JwtAuthError = 
        new(500, "5010", "An unexpected error occurred while processing encryption signatures");
            
    public static readonly CustomResponseDescriptor JwtSecretMissing = 
        new(500, "5015", "Critical configuration mismatch: JWT Secret Key not configured");

    public static readonly CustomResponseDescriptor FileStorageConfigError = 
        new(500, "5001", "File storage subsystem is misconfigured.");
            
    public static readonly CustomResponseDescriptor DataSeedingError = 
        new(500, "5002", "System data seeding failed.");
}
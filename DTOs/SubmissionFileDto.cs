namespace TraineeManagementApi.SubmissionFiles.DTOs;

public record SubmissionFileResponseDto 
(
    int Id,
    
    int SubmissionId,

    string OriginalFileName,

    string ContentType,

    long Size,

    string UploadedByUserId
);

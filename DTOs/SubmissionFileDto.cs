namespace TraineeManagementApi.SubmissionFiles.DTOs;

public record SubmissionFileResponseDto 
(
    int SubmissionId,

    string OriginalFileName,

    string ContentType,

    long Size,

    string UploadedByUserId
);

using TraineeManagement.Api.Data.SubmissionModel;
using TraineeManagement.Api.Data.CustomDataAnnotation;

namespace TraineeManagement.Api.Data.SubmissionDTO;

public record SubmissionCreateDto
(
    int TaskAssignmentId,

    [RequiredField]
    string SubmissionUrl,

    [RequiredField]
    string Notes,

    [RequiredField]
    DateOnly SubmittedDate,

    [ValidEnum(typeof(SubmissionStatus))]
    [RequiredField]
    SubmissionStatus? Status
);

public record SubmissionResponseDto
(
    int Id,

    int TaskAssignmentId,

    string SubmissionUrl,
    
    string Notes,
    
    DateOnly SubmittedDate,
    
    SubmissionStatus? Status
);
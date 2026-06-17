using System.ComponentModel.DataAnnotations;
using TraineeManagementApi.Submissions.Models;

namespace TraineeManagementApi.Submissions.DTOs;

public record SubmissionCreateDto
(
    int TaskAssignmentId,

    [Required]
    string SubmissionUrl,

    [Required]
    string Notes,

    [Required]
    DateOnly SubmittedDate,

    [EnumDataType(typeof(SubmissionStatus))]
    [Required]
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